// Workflow:
// Connect to SQL Server database
// Create a data table for bulk inserts
// Read a CSV file line by line so we don't load the entire file into memory
// Insert each line into the data table
// Bulk insert the data table into the database
// Close the connection
namespace ExtractDataFromCSV
{
    public static class Program
    {
        private static void Main()
        {
            try
            {
                string slnFolder = ServiceExtension.TryGetSolutionDirectoryInfo().FullName;

                ServiceExtension.UpdateAppSettingsWithDevelopmentValuesService("ConnectionStrings");

                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(slnFolder)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                // Start SQL Connection and open it
                //string sqlConnectionString = configuration["ConnectionStrings:Databasename01"];
                SqlConnection sqlConnection = new(configuration["ConnectionStrings:Databasename01"]);

                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                try
                {
                    DataTable dataTableIdentityDimension = new("dbo.Identity_Dimension");
                    DataTable dataTableInvoiceDimension = new("dbo.Invoices");
                    DataTable dataTableProductDimension = new("dbo.Product_Dimension");
                    DataTable dataTableSalesFactTable = new("dbo.Sales");
                    DataTable dataTablePurchasesFactTable = new("dbo.Purchases");
                    DataTable dataTableMovementsFactTable = new("dbo.Movements");

                    // Create the columns for the Identities table
                    dataTableIdentityDimension.Columns.Add("Nif", typeof(int));
                    dataTableIdentityDimension.Columns.Add("Name", typeof(string));
                    dataTableIdentityDimension.Columns.Add("PhoneNumber", typeof(string));
                    dataTableIdentityDimension.Columns.Add("Email", typeof(string));
                    dataTableIdentityDimension.Columns.Add("Address", typeof(string));
                    dataTableIdentityDimension.Columns.Add("PostalCode", typeof(string));
                    dataTableIdentityDimension.Columns.Add("City", typeof(string));
                    dataTableIdentityDimension.Columns.Add("Country_Region", typeof(string));
                    dataTableIdentityDimension.Columns.Add("Market", typeof(string));
                    dataTableIdentityDimension.Columns.Add("CompanyId", typeof(int));
                    dataTableIdentityDimension.Columns.Add("Type", typeof(string));

                    // Create the columns for the Invoices table
                    dataTableInvoiceDimension.Columns.Add("Number", typeof(string));
                    dataTableInvoiceDimension.Columns.Add("Type", typeof(string));
                    dataTableInvoiceDimension.Columns.Add("InvoiceDate", typeof(string));
                    dataTableInvoiceDimension.Columns.Add("CompanyId", typeof(int));

                    // Create a data structure that takes Number and Type as unique keys and Date of the document as the value
                    Dictionary<(string, string, int), string> invoiceDateKeys = new();

                    // Create the columns for the Products table
                    dataTableProductDimension.Columns.Add("Cod", typeof(string));
                    dataTableProductDimension.Columns.Add("Family", typeof(string));
                    dataTableProductDimension.Columns.Add("Description", typeof(string));
                    dataTableProductDimension.Columns.Add("PertentageOfVat", typeof(double));
                    dataTableProductDimension.Columns.Add("ExistingStock", typeof(int));
                    dataTableProductDimension.Columns.Add("PurchasingPrice", typeof(double));
                    dataTableProductDimension.Columns.Add("CompanyId", typeof(int));

                    // Create a dictionary to temporarily store the extracted product columns from different fields
                    // As they will come from different files
                    Dictionary<string, IList<string>> productColumns = new();

                    // Create the columns for the Sales Table
                    dataTableSalesFactTable.Columns.Add("NifClient", typeof(int));
                    dataTableSalesFactTable.Columns.Add("Number", typeof(string));
                    dataTableSalesFactTable.Columns.Add("Type", typeof(string));
                    dataTableSalesFactTable.Columns.Add("Cod", typeof(string));
                    dataTableSalesFactTable.Columns.Add("Quantity", typeof(int));
                    dataTableSalesFactTable.Columns.Add("NetAmount", typeof(double));
                    dataTableSalesFactTable.Columns.Add("GrossAmount", typeof(double));
                    dataTableSalesFactTable.Columns.Add("VAT", typeof(double));
                    dataTableSalesFactTable.Columns.Add("OriginalCurrency", typeof(string));
                    dataTableSalesFactTable.Columns.Add("ExchangeRate", typeof(double));
                    dataTableSalesFactTable.Columns.Add("NetAmountOriginalCurrency", typeof(double));
                    dataTableSalesFactTable.Columns.Add("GrossAmountOriginalCurrency", typeof(double));
                    dataTableSalesFactTable.Columns.Add("VATOriginalCurrency", typeof(double));
                    dataTableSalesFactTable.Columns.Add("CompanyId", typeof(int));

                    // Create the columns for the Purchases Table
                    dataTablePurchasesFactTable.Columns.Add("NifSupplier", typeof(int));
                    dataTablePurchasesFactTable.Columns.Add("Number", typeof(string));
                    dataTablePurchasesFactTable.Columns.Add("Type", typeof(string));
                    dataTablePurchasesFactTable.Columns.Add("Cod", typeof(string));
                    dataTablePurchasesFactTable.Columns.Add("Quantity", typeof(int));
                    dataTablePurchasesFactTable.Columns.Add("OurRef", typeof(string));
                    dataTablePurchasesFactTable.Columns.Add("NetAmount", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("GrossAmount", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("VAT", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("OriginalCurrency", typeof(string));
                    dataTablePurchasesFactTable.Columns.Add("ExchangeRate", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("NetAmountOriginalCurrency", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("GrossAmountOriginalCurrency", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("VATOriginalCurrency", typeof(double));
                    dataTablePurchasesFactTable.Columns.Add("CompanyId", typeof(int));

                    // Create the columns for the Movements Table
                    dataTableMovementsFactTable.Columns.Add("Cod", typeof(string));
                    dataTableMovementsFactTable.Columns.Add("Doc", typeof(string));
                    dataTableMovementsFactTable.Columns.Add("Type", typeof(string));
                    dataTableMovementsFactTable.Columns.Add("EntryQuantity", typeof(int));
                    dataTableMovementsFactTable.Columns.Add("ExitQuantity", typeof(int));
                    dataTableMovementsFactTable.Columns.Add("EntryValue", typeof(double));
                    dataTableMovementsFactTable.Columns.Add("ExitValue", typeof(double));
                    dataTableMovementsFactTable.Columns.Add("MovementValue", typeof(double));
                    dataTableMovementsFactTable.Columns.Add("ThirdParty", typeof(string));
                    dataTableMovementsFactTable.Columns.Add("CompanyId", typeof(int));

                    // Read the files in ..\Data
                    string filePath = Path.Combine(slnFolder, ".\\Data");

                    Console.WriteLine("Reading file with data from clients");

                    // Read the Clients and Suppliers CSV files and insert the data into the Identity Dimension table
                    using (var stream = File.Open($"{filePath}\\Clients.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 7 rows
                        for (int i = 0; i < 7; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            int nif = int.Parse(reader.GetString(2));
                            string name = reader.GetString(3);
                            string phoneNumber = reader.GetString(4);
                            string email = reader.GetString(6);
                            string address = reader.GetString(10);
                            string postalCode = reader.GetString(11);
                            string city = reader.GetString(12);
                            string countryRegion = reader.GetString(13);
                            string market = reader.GetString(1);

                            if (!dataTableIdentityDimension.AsEnumerable().Any(row => row.Field<int>("Nif") == nif && row.Field<int>("CompanyId") == companyId && row.Field<string>("Type") == "Client"))
                            {
                                dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market, companyId, "Client");
                            }
                        }
                    }

                    Console.WriteLine("Reading file with data from suppliers");

                    // Read the Suppliers Excel file and insert the data into the Identity Dimension table
                    using (var stream = File.Open($"{filePath}\\Suppliers.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 7 rows
                        for (int i = 0; i < 7; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            int nif = int.Parse(reader.GetString(2));
                            string name = reader.GetString(3);
                            string phoneNumber = reader.GetString(4);
                            string email = reader.GetString(5);
                            string address = reader.GetString(7);
                            string postalCode = reader.GetString(8);
                            string city = reader.GetString(9);
                            string countryRegion = reader.GetString(10);
                            string market = reader.GetString(1);

                            if (!dataTableIdentityDimension.AsEnumerable().Any(row => row.Field<int>("Nif") == nif && row.Field<int>("CompanyId") == companyId && row.Field<string>("Type") == "Supplier"))
                            {
                                dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market, companyId, "Supplier");
                            }
                        }
                    }

                    Console.WriteLine("Reading file with data from products");

                    // Read the Products Excel file and insert the data into the Product Dimension table
                    using (var stream = File.Open($"{filePath}\\Products.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 8 rows
                        for (int i = 0; i < 8; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            string cod = reader.GetString(2);
                            string family = reader.GetString(4);
                            string description = reader.GetString(5);
                            string vat = reader.GetString(11);
                            double percentageOfVat;

                            if (vat == "Normal")
                            {
                                percentageOfVat = 0.23;
                            }
                            else if (vat == "Intermediary")
                            {
                                percentageOfVat = 0.13;
                            }
                            else
                            {
                                percentageOfVat = 0.06;
                            }

                            double purchasingPrice = reader.GetDouble(10);

                            // Check if the key exists and append to the existing list, otherwise create a new list
                            if (!productColumns.ContainsKey("Cod"))
                                productColumns.Add("Cod", new List<string>());
                            productColumns["Cod"].Add(cod);

                            if (!productColumns.ContainsKey("Family"))
                                productColumns.Add("Family", new List<string>());
                            productColumns["Family"].Add(family);

                            if (!productColumns.ContainsKey("Description"))
                                productColumns.Add("Description", new List<string>());
                            productColumns["Description"].Add(description);

                            if (!productColumns.ContainsKey("PercentageOfVat"))
                                productColumns.Add("PercentageOfVat", new List<string>());
                            productColumns["PercentageOfVat"].Add(percentageOfVat.ToString());

                            if (!productColumns.ContainsKey("PurchasingPrice"))
                                productColumns.Add("PurchasingPrice", new List<string>());
                            productColumns["PurchasingPrice"].Add(purchasingPrice.ToString());

                            if (!productColumns.ContainsKey("CompanyId"))
                                productColumns.Add("CompanyId", new List<string>());
                            productColumns["CompanyId"].Add(companyId.ToString());
                        }
                    }

                    Console.WriteLine("Reading file with data from sale invoices");

                    // Read the Sales Invoices Excel file and insert the data into the Sales Fact Table
                    using (var stream = File.Open($"{filePath}\\SaleInvoices.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 12 rows
                        for (int i = 0; i < 12; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            int nifClient = int.Parse(reader.GetString(1));
                            string dateKey = reader.GetString(3);
                            dateKey = dateKey.Replace("/", "-");
                            string number = reader.GetString(5);
                            string type = reader.GetString(4);
                            string cod = reader.GetString(6);
                            int quantity = (int)reader.GetDouble(8);
                            double netAmount = (reader.GetDouble(9));
                            double grossAmount = (reader.GetDouble(10));
                            double vat = (reader.GetDouble(11));
                            string originalCurrency = reader.GetString(12);
                            double exchangeRate = reader.GetDouble(13);
                            double netAmountOriginalCurrency = reader.GetDouble(14);
                            double grossAmountOriginalCurrency = reader.GetDouble(15);
                            double vatOriginalCurrency = reader.GetDouble(16);

                            string[] dateKeyParts = dateKey.Split('-');
                            dateKeyParts[1] = dateKeyParts[1].Length < 2 ? "0" + dateKeyParts[1] : dateKeyParts[1];
                            dateKeyParts[2] = dateKeyParts[2].Length < 2 ? "0" + dateKeyParts[2] : dateKeyParts[2];

                            dateKey = dateKeyParts[0] + "/" + dateKeyParts[1] + "/" + dateKeyParts[2];

                            if (!invoiceDateKeys.ContainsKey((number, type, companyId)))
                            {
                                invoiceDateKeys.Add((number, type, companyId), dateKey);
                            }

                            if (!dataTableSalesFactTable.AsEnumerable().Any(row => row.Field<string>("Number") == number && row.Field<string>("Cod") == cod && row.Field<int>("CompanyId") == companyId))
                            {
                                dataTableSalesFactTable.Rows.Add(
                                    nifClient, number, type,
                                    cod, quantity, netAmount,
                                    grossAmount, vat,
                                    originalCurrency, exchangeRate,
                                    netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency, companyId
                                );
                            }
                        }
                    }

                    Console.WriteLine("Reading file with data from purchase invoices");

                    // Read the Purchase Invoices Excel file and insert the data into the Purchases Fact Table
                    using (var stream = File.Open($"{filePath}\\PurchaseInvoices.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 8 rows
                        for (int i = 0; i < 8; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            int nifSupplier = int.Parse(reader.GetString(1));
                            string dateKey = reader.GetString(3);
                            dateKey = dateKey.Replace("/", "-");
                            string number = reader.GetString(5);
                            string type = reader.GetString(4);
                            string cod = reader.GetString(7);
                            int quantity = (int)reader.GetDouble(9);
                            string ourRef = reader.GetString(6);
                            double netAmount = reader.GetDouble(10);
                            double grossAmount = reader.GetDouble(11);
                            double vat = reader.GetDouble(12);
                            string originalCurrency = reader.GetString(13);
                            double exchangeRate = reader.GetDouble(14);
                            double netAmountOriginalCurrency = reader.GetDouble(15);
                            double grossAmountOriginalCurrency = reader.GetDouble(16);
                            double vatOriginalCurrency = reader.GetDouble(17);

                            string[] dateKeyParts = dateKey.Split('-');
                            dateKeyParts[1] = dateKeyParts[1].Length < 2 ? "0" + dateKeyParts[1] : dateKeyParts[1];
                            dateKeyParts[2] = dateKeyParts[2].Length < 2 ? "0" + dateKeyParts[2] : dateKeyParts[2];

                            dateKey = dateKeyParts[0] + "/" + dateKeyParts[1] + "/" + dateKeyParts[2];

                            if (!invoiceDateKeys.ContainsKey((number, type, companyId)))
                            {
                                invoiceDateKeys.Add((number, type, companyId), dateKey);
                            }

                            if (!dataTablePurchasesFactTable.AsEnumerable().Any(row => row.Field<string>("Number") == number && row.Field<string>("Cod") == cod && row.Field<int>("CompanyId") == companyId))
                            {
                                dataTablePurchasesFactTable.Rows.Add(
                                    nifSupplier, number, type,
                                    cod, quantity, ourRef,
                                    netAmount, grossAmount, vat,
                                    originalCurrency, exchangeRate,
                                    netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency, companyId
                                );
                            }
                        }
                    }

                    Console.WriteLine("Reading file with data from stock movements");

                    // Read the Movements Excel file and insert the data into the Movements Fact Table
                    using (var stream = File.Open($"{filePath}\\StockMovements.xlsx", FileMode.Open, FileAccess.Read))
                    {
                        using var reader = ExcelReaderFactory.CreateReader(stream);

                        // Ignore first 10 rows
                        for (int i = 0; i < 10; i++)
                        {
                            reader.Read();
                        }

                        while (reader.Read()) // Each Read() call will move to the next row
                        {
                            // Get the values from the current row...
                            int companyId = (int)reader.GetDouble(0);
                            string cod = reader.GetString(1);
                            string dateKey = reader.GetString(3);
                            dateKey = dateKey.Replace("/", "-");
                            string doc = reader.GetString(6);
                            string type = reader.GetString(5);
                            int entryQuantity = (int)reader.GetDouble(7);
                            int exitQuantity = (int)reader.GetDouble(8);
                            int existingQuantity = (int)reader.GetDouble(9);
                            double entryValue = reader.GetDouble(10);
                            double exitValue = reader.GetDouble(11);
                            double movementValue = reader.GetDouble(12);
                            string thirdParty = reader.GetString(15);

                            string[] dateKeyParts = dateKey.Split('-');
                            dateKeyParts[1] = dateKeyParts[1].Length < 2 ? "0" + dateKeyParts[1] : dateKeyParts[1];
                            dateKeyParts[2] = dateKeyParts[2].Length < 2 ? "0" + dateKeyParts[2] : dateKeyParts[2];

                            dateKey = dateKeyParts[0] + "/" + dateKeyParts[1] + "/" + dateKeyParts[2];

                            if (!invoiceDateKeys.ContainsKey((doc, type, companyId)))
                            {
                                if (doc.IsNullOrEmpty())
                                {
                                    doc = "PreviousStock";
                                }

                                if (type.Equals("Entrada") || type.IsNullOrEmpty() || type.Equals("Stock anterior"))
                                {
                                    type = "In";
                                }

                                if (type.Equals("Saída"))
                                {
                                    type = "Out";
                                }

                                if (!invoiceDateKeys.ContainsKey((doc, type, companyId)))
                                {
                                    invoiceDateKeys.Add((doc, type, companyId), dateKey);
                                }
                            }

                            // For each unique cod, keep track of the entryQuantity and exitQuantity values so we can get the existing stock for each product
                            if (!productColumns.TryGetValue(cod, out IList<string>? value))
                            {
                                productColumns.Add(cod, new List<string> { existingQuantity.ToString() });
                            }
                            else
                            {
                                value[0] = existingQuantity.ToString();
                            }

                            if (dataTableMovementsFactTable.AsEnumerable().Any(row => row.Field<string>("Doc") == doc && row.Field<string>("Type") == type && row.Field<string>("Cod") == cod && row.Field<int>("CompanyId") == companyId))
                            {
                                continue;
                            }

                            dataTableMovementsFactTable.Rows.Add(
                                    cod, doc, type,
                                    entryQuantity, exitQuantity, entryValue,
                                    exitValue, movementValue, thirdParty, companyId
                            );
                        }
                    }

                    Console.WriteLine("Organizing data read so it will be written to the tables in 3rd normal form");

                    // For each cod in productColumns, extract the value and search for the corresponding column to extract the existing stock of each product and add it to a new column "ExistingStock"
                    for (int i = 0; i < productColumns["Cod"].Count; i++)
                    {
                        string cod = productColumns["Cod"][i];

                        // Get the existing stock for the product
                        if (productColumns.TryGetValue(cod, out IList<string>? value))
                        {
                            int existingStock = int.Parse(value[0]);

                            if (!productColumns.ContainsKey("ExistingStock"))
                                productColumns.Add("ExistingStock", new List<string>());
                            productColumns["ExistingStock"].Add(existingStock.ToString());
                        }
                    }

                    // For each column in the productColumns dictionary, add the values to the Product Dimension table
                    for (int i = 0; i < productColumns["Cod"].Count; i++)
                    {
                        dataTableProductDimension.Rows.Add(productColumns["Cod"][i], productColumns["Family"][i], productColumns["Description"][i], double.Parse(productColumns["PercentageOfVat"][i]), (productColumns["ExistingStock"][i]), double.Parse(productColumns["PurchasingPrice"][i]), int.Parse(productColumns["CompanyId"][i]));
                    }

                    // For each unique invoice, get the corresponding Date_Key and add it to the Invoice Dimension table
                    foreach (var invoice in invoiceDateKeys)
                    {
                        dataTableInvoiceDimension.Rows.Add(invoice.Key.Item1, invoice.Key.Item2, invoice.Value, invoice.Key.Item3);
                    }

                    // Truncate all tables
                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Clients_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Suppliers_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Quartile_Products_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Clients_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Suppliers_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Month_Products_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Clients_Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Suppliers_Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Daily_Products_Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Sales_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Purchases_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Movements_FactTable", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    // Delete all rows from the tables
                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Date_Dimension", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Identity_Dimension", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Product_Dimension", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Sales", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Purchases", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.Movements", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Invoices", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Products", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand sqlCommand = new("DELETE FROM dbo.Identities", sqlConnection, sqlTransaction))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Writing data to the tables in 3rd normal form");

                    // SqlBulkCopy for the Identity Dimension table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Identities";

                        bulkCopy.ColumnMappings.Add("Nif", "[Nif]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");
                        bulkCopy.ColumnMappings.Add("Name", "[Name]");
                        bulkCopy.ColumnMappings.Add("PhoneNumber", "[PhoneNumber]");
                        bulkCopy.ColumnMappings.Add("Email", "[Email]");
                        bulkCopy.ColumnMappings.Add("Address", "[Address]");
                        bulkCopy.ColumnMappings.Add("PostalCode", "[PostalCode]");
                        bulkCopy.ColumnMappings.Add("City", "[City]");
                        bulkCopy.ColumnMappings.Add("Country_Region", "[Country_Region]");
                        bulkCopy.ColumnMappings.Add("Market", "[Market]");
                        bulkCopy.ColumnMappings.Add("Type", "[Type]");

                        bulkCopy.WriteToServer(dataTableIdentityDimension);
                    }

                    // SqlBulkCopy for the Invoice Dimension table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Invoices";

                        bulkCopy.ColumnMappings.Add("Number", "[Number]");
                        bulkCopy.ColumnMappings.Add("Type", "[Type]");
                        bulkCopy.ColumnMappings.Add("InvoiceDate", "[InvoiceDate]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");

                        bulkCopy.WriteToServer(dataTableInvoiceDimension);
                    }

                    // SqlBulkCopy for the Product Dimension table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Products";

                        bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                        bulkCopy.ColumnMappings.Add("Family", "[Family]");
                        bulkCopy.ColumnMappings.Add("Description", "[Description]");
                        bulkCopy.ColumnMappings.Add("PertentageOfVat", "[PercentageofVAT]");
                        bulkCopy.ColumnMappings.Add("ExistingStock", "[ExistingStock]");
                        bulkCopy.ColumnMappings.Add("PurchasingPrice", "[PurchasingPrice]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");

                        bulkCopy.WriteToServer(dataTableProductDimension);
                    }

                    // SqlBulkCopy for the Sales Fact Table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Sales";

                        bulkCopy.ColumnMappings.Add("NifClient", "[NifClient]");
                        bulkCopy.ColumnMappings.Add("Number", "[Number]");
                        bulkCopy.ColumnMappings.Add("Type", "[Type]");
                        bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                        bulkCopy.ColumnMappings.Add("Quantity", "[Quantity]");
                        bulkCopy.ColumnMappings.Add("NetAmount", "[NetAmount]");
                        bulkCopy.ColumnMappings.Add("GrossAmount", "[GrossAmount]");
                        bulkCopy.ColumnMappings.Add("VAT", "[VAT]");
                        bulkCopy.ColumnMappings.Add("OriginalCurrency", "[OriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("ExchangeRate", "[ExchangeRate]");
                        bulkCopy.ColumnMappings.Add("NetAmountOriginalCurrency", "[NetAmountOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("GrossAmountOriginalCurrency", "[GrossAmountOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("VATOriginalCurrency", "[VATOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");

                        bulkCopy.WriteToServer(dataTableSalesFactTable);
                    }

                    // SqlBulkCopy for the Purchases Fact Table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Purchases";

                        bulkCopy.ColumnMappings.Add("NifSupplier", "[NifSupplier]");
                        bulkCopy.ColumnMappings.Add("Number", "[Number]");
                        bulkCopy.ColumnMappings.Add("Type", "[Type]");
                        bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                        bulkCopy.ColumnMappings.Add("Quantity", "[Quantity]");
                        bulkCopy.ColumnMappings.Add("OurRef", "[OurRef]");
                        bulkCopy.ColumnMappings.Add("NetAmount", "[NetAmount]");
                        bulkCopy.ColumnMappings.Add("GrossAmount", "[GrossAmount]");
                        bulkCopy.ColumnMappings.Add("VAT", "[VAT]");
                        bulkCopy.ColumnMappings.Add("OriginalCurrency", "[OriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("ExchangeRate", "[ExchangeRate]");
                        bulkCopy.ColumnMappings.Add("NetAmountOriginalCurrency", "[NetAmountOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("GrossAmountOriginalCurrency", "[GrossAmountOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("VATOriginalCurrency", "[VATOriginalCurrency]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");

                        bulkCopy.WriteToServer(dataTablePurchasesFactTable);
                    }

                    // SqlBulkCopy for the Movements Fact Table
                    using (SqlBulkCopy bulkCopy = new(sqlConnection, SqlBulkCopyOptions.FireTriggers, sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Movements";

                        bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                        bulkCopy.ColumnMappings.Add("Doc", "[Document]");
                        bulkCopy.ColumnMappings.Add("Type", "[Type]");
                        bulkCopy.ColumnMappings.Add("EntryQuantity", "[EntryQuantity]");
                        bulkCopy.ColumnMappings.Add("ExitQuantity", "[ExitQuantity]");
                        bulkCopy.ColumnMappings.Add("EntryValue", "[EntryValue]");
                        bulkCopy.ColumnMappings.Add("ExitValue", "[ExitValue]");
                        bulkCopy.ColumnMappings.Add("MovementValue", "[MovementValue]");
                        bulkCopy.ColumnMappings.Add("ThirdParty", "[ThirdParty]");
                        bulkCopy.ColumnMappings.Add("CompanyId", "[CompanyId]");

                        bulkCopy.WriteToServer(dataTableMovementsFactTable);
                    }

                    // Commit the transaction
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }

                // Finally, close the connection
                sqlConnection.Close();

                Console.WriteLine("Process finished!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //Console.WriteLine(ex.Message);
            }
        }
    }
}