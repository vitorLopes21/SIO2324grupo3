// Workflow:
// Connect to SQL Server database
// Create a data table for bulk inserts
// Read a CSV file line by line so we don't load the entire file into memory
// Insert each line into the data table
// Bulk insert the data table into the database
// Close the connection

using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;

namespace ExtractDataFromCSV
{
    public static class Program
    {
        private static void Main(string[] args)
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
                SqlConnection sqlConnection = new(
                    configuration["ConnectionStrings:Databasename01"]
                );

                sqlConnection.Open();

                DataTable dataTableDateDimension = new("dbo.Date_Dimension");
                DataTable dataTableIdentityDimension = new("dbo.Identity_Dimension");
                DataTable dataTableInvoiceDimension = new("dbo.Invoice_Dimension");
                DataTable dataTableProductDimension = new("dbo.Product_Dimension");
                DataTable dataTableSalesFactTable = new("dbo.SalesFactTable");
                DataTable dataTablePurchasesFactTable = new("dbo.PurchasesFactTable");
                DataTable dataTableMovementsFactTable = new("dbo.MovementsFactTable");

                // Create the columns for the Date Dimension table
                dataTableDateDimension.Columns.Add("Date_Key", typeof(string));
                dataTableDateDimension.Columns.Add("Day", typeof(int));
                dataTableDateDimension.Columns.Add("Month", typeof(int));
                dataTableDateDimension.Columns.Add("Year", typeof(int));

                // Unique dates set
                HashSet<string> uniqueDates = new();

                // Create the columns for the Identity Dimension table
                dataTableIdentityDimension.Columns.Add("NIF", typeof(int));
                dataTableIdentityDimension.Columns.Add("Name", typeof(string));
                dataTableIdentityDimension.Columns.Add("PhoneNumber", typeof(string));
                dataTableIdentityDimension.Columns.Add("Email", typeof(string));
                dataTableIdentityDimension.Columns.Add("Address", typeof(string));
                dataTableIdentityDimension.Columns.Add("PostalCode", typeof(string));
                dataTableIdentityDimension.Columns.Add("City", typeof(string));
                dataTableIdentityDimension.Columns.Add("Country_Region", typeof(string));
                dataTableIdentityDimension.Columns.Add("Market", typeof(string));

                // Create the columns for the Invoice Dimension table
                dataTableInvoiceDimension.Columns.Add("Number", typeof(string));
                dataTableInvoiceDimension.Columns.Add("Type", typeof(string));
                dataTableInvoiceDimension.Columns.Add("Date_Key", typeof(string));

                // Create a data structure that takes Number and Type as unique keys and Date_Key as the value
                Dictionary<(string, string), string> invoiceDateKeys = new();

                // Create the columns for the Product Dimension table
                dataTableProductDimension.Columns.Add("Cod", typeof(string));
                dataTableProductDimension.Columns.Add("Family", typeof(string));
                dataTableProductDimension.Columns.Add("Description", typeof(string));
                dataTableProductDimension.Columns.Add("PertentageOfVat", typeof(double));
                dataTableProductDimension.Columns.Add("ExistingStock", typeof(int));
                dataTableProductDimension.Columns.Add("PurchasingPrice", typeof(double));

                // Create a dictionary to temporarily store the extracted product columns from different fields
                // As they will come from different files
                Dictionary<string, IList<string>> productColumns = new();

                // Create the columns for the Sales Fact Table
                dataTableSalesFactTable.Columns.Add("NIF_Client", typeof(int));
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

                // Create the columns for the Purchases Fact Table
                dataTablePurchasesFactTable.Columns.Add("NIF_Client", typeof(int));
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

                // Create the columns for the Movements Fact Table
                dataTableMovementsFactTable.Columns.Add("Cod", typeof(string));
                dataTableMovementsFactTable.Columns.Add("Doc", typeof(string));
                dataTableMovementsFactTable.Columns.Add("Type", typeof(string));
                dataTableMovementsFactTable.Columns.Add("EntryQuantity", typeof(int));
                dataTableMovementsFactTable.Columns.Add("ExitQuantity", typeof(int));
                dataTableMovementsFactTable.Columns.Add("EntryValue", typeof(double));
                dataTableMovementsFactTable.Columns.Add("ExitValue", typeof(double));
                dataTableMovementsFactTable.Columns.Add("MovementValue", typeof(double));
                dataTableMovementsFactTable.Columns.Add("ThirdParty", typeof(string));

                // Read the files in ..\Data
                string filePath = Path.Combine(slnFolder, ".\\Data");

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
                        int nif = int.Parse(reader.GetString(1));
                        // If the NIF does not exist in the DataTable, add a new row
                        string name = reader.GetString(2);
                        string phoneNumber = reader.GetString(3);
                        string email = reader.GetString(4);
                        string address = reader.GetString(6);
                        string postalCode = reader.GetString(7);
                        string city = reader.GetString(8);
                        string countryRegion = reader.GetString(9);
                        string market = reader.GetString(0);

                        if (!dataTableIdentityDimension.AsEnumerable().Any(row => row.Field<int>("NIF") == nif))
                        {
                            dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market);
                        }
                    }
                }

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
                        int nif = int.Parse(reader.GetString(1));
                        string name = reader.GetString(2);
                        string phoneNumber = reader.GetString(3);
                        string email = reader.GetString(4);
                        string address = reader.GetString(6);
                        string postalCode = reader.GetString(7);
                        string city = reader.GetString(8);
                        string countryRegion = reader.GetString(9);
                        string market = reader.GetString(0);

                        if (!dataTableIdentityDimension.AsEnumerable().Any(row => row.Field<int>("NIF") == nif))
                        {
                            dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market);
                        }
                    }
                }

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
                        string cod = reader.GetString(1);
                        string family = reader.GetString(3);
                        string description = reader.GetString(4);
                        string vat = reader.GetString(10);
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

                        double purchasingPrice = reader.GetDouble(9);

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
                    }
                }

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
                        int nifClient = int.Parse(reader.GetString(0));
                        string dateKey = reader.GetString(2);
                        dateKey = dateKey.Replace("/", "-");
                        string number = reader.GetString(4);
                        string type = reader.GetString(3);
                        string cod = reader.GetString(5);
                        int quantity = (int)reader.GetDouble(7);
                        double netAmount = (reader.GetDouble(8));
                        double grossAmount = (reader.GetDouble(9));
                        double vat = (reader.GetDouble(10));
                        string originalCurrency = reader.GetString(11);
                        double exchangeRate = reader.GetDouble(12);
                        double netAmountOriginalCurrency = reader.GetDouble(13);
                        double grossAmountOriginalCurrency = reader.GetDouble(14);
                        double vatOriginalCurrency = reader.GetDouble(15);

                        _ = uniqueDates.Add(dateKey);

                        dateKey = dateKey.Replace("-", "");

                        if (!invoiceDateKeys.ContainsKey((number, type)))
                        {
                            invoiceDateKeys.Add((number, type), dateKey);
                        }

                        if (!dataTableSalesFactTable.AsEnumerable().Any(row => row.Field<string>("Number") == number && row.Field<string>("Cod") == cod))
                        {
                            dataTableSalesFactTable.Rows.Add(
                                nifClient, number, type,
                                cod, quantity, netAmount,
                                grossAmount, vat,
                                originalCurrency, exchangeRate,
                                netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency
                            );
                        }
                    }
                }

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
                        int nifClient = int.Parse(reader.GetString(0));
                        string dateKey = reader.GetString(2);
                        dateKey = dateKey.Replace("/", "-");
                        string number = reader.GetString(4);
                        string type = reader.GetString(3);
                        string cod = reader.GetString(6);
                        int quantity = (int)reader.GetDouble(8);
                        string ourRef = reader.GetString(5);
                        double netAmount = reader.GetDouble(9);
                        double grossAmount = reader.GetDouble(10);
                        double vat = reader.GetDouble(11);
                        string originalCurrency = reader.GetString(12);
                        double exchangeRate = reader.GetDouble(13);
                        double netAmountOriginalCurrency = reader.GetDouble(14);
                        double grossAmountOriginalCurrency = reader.GetDouble(15);
                        double vatOriginalCurrency = reader.GetDouble(16);

                        _ = uniqueDates.Add(dateKey);

                        dateKey = dateKey.Replace("-", "");

                        if (!invoiceDateKeys.ContainsKey((number, type)))
                        {
                            invoiceDateKeys.Add((number, type), dateKey);
                        }

                        if (!dataTablePurchasesFactTable.AsEnumerable().Any(row => row.Field<string>("Number") == number && row.Field<string>("Cod") == cod))
                        {
                            dataTablePurchasesFactTable.Rows.Add(
                                nifClient, number, type,
                                cod, quantity, ourRef,
                                netAmount, grossAmount, vat,
                                originalCurrency, exchangeRate,
                                netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency
                            );
                        }
                    }
                }

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
                        string cod = reader.GetString(0);
                        string dateKey = reader.GetString(2);
                        dateKey = dateKey.Replace("/", "-");
                        string doc = reader.GetString(5);
                        string type = reader.GetString(4);
                        int entryQuantity = (int)reader.GetDouble(6);
                        int exitQuantity = (int)reader.GetDouble(7);
                        int existingQuantity = (int)reader.GetDouble(8);
                        double entryValue = reader.GetDouble(9);
                        double exitValue = reader.GetDouble(10);
                        double movementValue = reader.GetDouble(11);
                        string thirdParty = reader.GetString(14);

                        _ = uniqueDates.Add(dateKey);

                        dateKey = dateKey.Replace("-", "");

                        if (!invoiceDateKeys.ContainsKey((doc, type)))
                        {
                            if (doc.IsNullOrEmpty() || type.IsNullOrEmpty())
                            {
                                continue;
                            }

                            if (type.Equals("Entrada"))
                            {
                                type = "In";
                            }

                            if (type.Equals("Saída"))
                            {
                                type = "Out";
                            }

                            if (!invoiceDateKeys.ContainsKey((doc, type)))
                            {
                                invoiceDateKeys.Add((doc, type), dateKey);
                            }
                        }

                        // For each unique cod, keep track of the entryQuantity and exitQuantity values so we can get the existing stock for each product
                        if (!productColumns.TryGetValue(cod, out IList<string>? value))
                        {
                            productColumns.Add(cod, new List<string> { existingQuantity.ToString() });
                        }
                        else
                        {
                            int existingQuantityValue = int.Parse(value[0]);
                            existingQuantityValue += existingQuantity;
                            value[0] = existingQuantityValue.ToString();
                        }

                        if (dataTableMovementsFactTable.AsEnumerable().Any(row => row.Field<string>("Doc") == doc && row.Field<string>("Type") == type))
                        {
                            continue;
                        }

                        dataTableMovementsFactTable.Rows.Add(
                                cod, doc, type,
                                entryQuantity, exitQuantity, entryValue,
                                exitValue, movementValue, thirdParty
                        );
                    }
                }

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
                    dataTableProductDimension.Rows.Add(productColumns["Cod"][i], productColumns["Family"][i], productColumns["Description"][i], double.Parse(productColumns["PercentageOfVat"][i]), (productColumns["ExistingStock"][i]), double.Parse(productColumns["PurchasingPrice"][i]));
                }

                for (int i = 0; i < uniqueDates.Count; i++)
                {
                    string date = uniqueDates.ElementAt(i);

                    // Get the day, month, and year from the date
                    string[] dateParts = date.Split('-');

                    // 20240101 - 01 - 01 - 2024
                    date = dateParts[0] + dateParts[1] + dateParts[2];
                    int day = int.Parse(dateParts[2]);
                    int month = int.Parse(dateParts[1]);
                    int year = int.Parse(dateParts[0]);

                    if (!dataTableDateDimension.AsEnumerable().Any(row => row.Field<string>("Date_Key") == date))
                    {
                        dataTableDateDimension.Rows.Add(date, day, month, year);
                    }
                }

                // For each unique invoice, get the corresponding Date_Key and add it to the Invoice Dimension table
                foreach (var invoice in invoiceDateKeys)
                {
                    dataTableInvoiceDimension.Rows.Add(invoice.Key.Item1, invoice.Key.Item2, invoice.Value);
                }

                using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.SalesFactTable", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.PurchasesFactTable", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                using (SqlCommand sqlCommand = new("TRUNCATE TABLE dbo.MovementsFactTable", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                using (SqlCommand sqlCommand = new("DELETE FROM dbo.Invoice_Dimension", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                // Delete all rows from the tables
                using (SqlCommand sqlCommand = new("DELETE FROM dbo.Date_Dimension", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                using (SqlCommand sqlCommand = new("DELETE FROM dbo.Identity_Dimension", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                using (SqlCommand sqlCommand = new("DELETE FROM dbo.Product_Dimension", sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }

                // SqlBulkCopy for the Date Dimension table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.Date_Dimension";

                    bulkCopy.ColumnMappings.Add("Date_Key", "[Date_Key]");
                    bulkCopy.ColumnMappings.Add("Day", "[Day]");
                    bulkCopy.ColumnMappings.Add("Month", "[Month]");
                    bulkCopy.ColumnMappings.Add("Year", "[Year]");

                    bulkCopy.WriteToServer(dataTableDateDimension);
                }

                // SqlBulkCopy for the Identity Dimension table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.Identity_Dimension";

                    bulkCopy.ColumnMappings.Add("NIF", "[NIF]");
                    bulkCopy.ColumnMappings.Add("Name", "[Name]");
                    bulkCopy.ColumnMappings.Add("PhoneNumber", "[PhoneNumber]");
                    bulkCopy.ColumnMappings.Add("Email", "[Email]");
                    bulkCopy.ColumnMappings.Add("Address", "[Address]");
                    bulkCopy.ColumnMappings.Add("PostalCode", "[PostalCode]");
                    bulkCopy.ColumnMappings.Add("City", "[City]");
                    bulkCopy.ColumnMappings.Add("Country_Region", "[Country_Region]");
                    bulkCopy.ColumnMappings.Add("Market", "[Market]");

                    bulkCopy.WriteToServer(dataTableIdentityDimension);
                }

                // SqlBulkCopy for the Invoice Dimension table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.Invoice_Dimension";

                    bulkCopy.ColumnMappings.Add("Number", "[Number]");
                    bulkCopy.ColumnMappings.Add("Type", "[Type]");
                    bulkCopy.ColumnMappings.Add("Date_Key", "[Date_Key]");

                    bulkCopy.WriteToServer(dataTableInvoiceDimension);
                }

                // SqlBulkCopy for the Product Dimension table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.Product_Dimension";

                    bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                    bulkCopy.ColumnMappings.Add("Family", "[Family]");
                    bulkCopy.ColumnMappings.Add("Description", "[Description]");
                    bulkCopy.ColumnMappings.Add("PertentageOfVat", "[PercentageofVAT]");
                    bulkCopy.ColumnMappings.Add("ExistingStock", "[ExistingStock]");
                    bulkCopy.ColumnMappings.Add("PurchasingPrice", "[PurchasingPrice]");

                    bulkCopy.WriteToServer(dataTableProductDimension);
                }

                // SqlBulkCopy for the Sales Fact Table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.SalesFactTable";

                    bulkCopy.ColumnMappings.Add("NIF_Client", "[NIF_Client]");
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

                    bulkCopy.WriteToServer(dataTableSalesFactTable);
                }

                // SqlBulkCopy for the Purchases Fact Table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.PurchasesFactTable";

                    bulkCopy.ColumnMappings.Add("NIF_Client", "[NIF_Client]");
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

                    bulkCopy.WriteToServer(dataTablePurchasesFactTable);
                }

                // SqlBulkCopy for the Movements Fact Table
                using (SqlBulkCopy bulkCopy = new(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "dbo.MovementsFactTable";

                    bulkCopy.ColumnMappings.Add("Cod", "[Cod]");
                    bulkCopy.ColumnMappings.Add("Doc", "[Document]");
                    bulkCopy.ColumnMappings.Add("Type", "[Type]");
                    bulkCopy.ColumnMappings.Add("EntryQuantity", "[EntryQuantity]");
                    bulkCopy.ColumnMappings.Add("ExitQuantity", "[ExitQuantity]");
                    bulkCopy.ColumnMappings.Add("EntryValue", "[EntryValue]");
                    bulkCopy.ColumnMappings.Add("ExitValue", "[ExitValue]");
                    bulkCopy.ColumnMappings.Add("MovementValue", "[MovementValue]");
                    bulkCopy.ColumnMappings.Add("ThirdParty", "[ThirdParty]");

                    bulkCopy.WriteToServer(dataTableMovementsFactTable);
                }

                // Finally, close the connection
                sqlConnection.Close();

                Console.WriteLine("Data inserted successfully!");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
        }
    }
}