// Workflow:
// Connect to SQL Server database
// Create a data table for bulk inserts
// Read a CSV file line by line so we don't load the entire file into memory
// Insert each line into the data table
// Bulk insert the data table into the database
// Close the connection

SqlConnection connection = new(
    new SqlConnectionStringBuilder
    {
        DataSource = "servername01.database.windows.net",
        InitialCatalog = "databasename01",
        UserID = "AppsLogin",
        Password = "SioDb2024",
        ConnectTimeout = 30,
        TrustServerCertificate = false,
        Encrypt = true,
        MultipleActiveResultSets = false,
        PersistSecurityInfo = false
    }.ConnectionString
);

connection.Open();

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

// Create the columns for the Product Dimension table
dataTableProductDimension.Columns.Add("Cod", typeof(string));
dataTableProductDimension.Columns.Add("Family", typeof(string));
dataTableProductDimension.Columns.Add("Description", typeof(string));
dataTableProductDimension.Columns.Add("PertentageOfVat", typeof(decimal));
dataTableProductDimension.Columns.Add("ExistingStock", typeof(int));
dataTableProductDimension.Columns.Add("PurchasingPrice", typeof(float));
dataTableProductDimension.Columns.Add("StockValue", typeof(float));

// Create a dictionary to temporarily store the extracted product columns from different fields
// As they will come from different files
Dictionary<string, IList<string>> productColumns = new();

// Create the columns for the Sales Fact Table
dataTableSalesFactTable.Columns.Add("NIF_Client", typeof(int));
dataTableSalesFactTable.Columns.Add("Number", typeof(string));
dataTableSalesFactTable.Columns.Add("Type", typeof(string));
dataTableSalesFactTable.Columns.Add("Cod", typeof(string));
dataTableSalesFactTable.Columns.Add("Quantity", typeof(int));
dataTableSalesFactTable.Columns.Add("NetAmount", typeof(float));
dataTableSalesFactTable.Columns.Add("GrossAmount", typeof(float));
dataTableSalesFactTable.Columns.Add("VAT", typeof(float));
dataTableSalesFactTable.Columns.Add("OriginalCurrency", typeof(string));
dataTableSalesFactTable.Columns.Add("ExchangeRate", typeof(float));
dataTableSalesFactTable.Columns.Add("NetAmountOriginalCurrency", typeof(int));
dataTableSalesFactTable.Columns.Add("GrossAmountOriginalCurrency", typeof(int));
dataTableSalesFactTable.Columns.Add("VATOriginalCurrency", typeof(int));

// Create the columns for the Purchases Fact Table
dataTablePurchasesFactTable.Columns.Add("NIF_Client", typeof(int));
dataTablePurchasesFactTable.Columns.Add("Number", typeof(string));
dataTablePurchasesFactTable.Columns.Add("Type", typeof(string));
dataTablePurchasesFactTable.Columns.Add("Cod", typeof(string));
dataTablePurchasesFactTable.Columns.Add("Quantity", typeof(int));
dataTablePurchasesFactTable.Columns.Add("OurRef", typeof(string));
dataTablePurchasesFactTable.Columns.Add("NetAmount", typeof(float));
dataTablePurchasesFactTable.Columns.Add("GrossAmount", typeof(float));
dataTablePurchasesFactTable.Columns.Add("VAT", typeof(float));
dataTablePurchasesFactTable.Columns.Add("OriginalCurrency", typeof(string));
dataTablePurchasesFactTable.Columns.Add("ExchangeRate", typeof(float));
dataTablePurchasesFactTable.Columns.Add("NetAmountOriginalCurrency", typeof(int));
dataTablePurchasesFactTable.Columns.Add("GrossAmountOriginalCurrency", typeof(int));
dataTablePurchasesFactTable.Columns.Add("VATOriginalCurrency", typeof(int));

// Create the columns for the Movements Fact Table
dataTableMovementsFactTable.Columns.Add("Cod", typeof(string));
dataTableMovementsFactTable.Columns.Add("Doc", typeof(string));
dataTableMovementsFactTable.Columns.Add("Type", typeof(string));
dataTableMovementsFactTable.Columns.Add("EntryQuantity", typeof(int));
dataTableMovementsFactTable.Columns.Add("ExitQuantity", typeof(int));
dataTableMovementsFactTable.Columns.Add("EntryValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("ExitValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("MovementValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("ThirdParty", typeof(string));

// Read the files in ..\Data
string? directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException("Could not find the directory.");

string filePath = Path.Combine(directory, "..\\Data");

// Read the Clients and Suppliers CSV files and insert the data into the Identity Dimension table
using (var stream = File.Open($"{filePath}\\Clients.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);
    while (reader.Read()) // Each Read() call will move to the next row
    {
        // Get the values from the current row...
        string nif = reader.GetString(1);
        string name = reader.GetString(2);
        string phoneNumber = reader.GetString(3);
        string email = reader.GetString(5);
        string address = reader.GetString(9);
        string postalCode = reader.GetString(10);
        string city = reader.GetString(11);
        string countryRegion = reader.GetString(12);
        string market = reader.GetString(0);

        dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market);
    }
}

// Read the Suppliers Excel file and insert the data into the Identity Dimension table
using (var stream = File.Open($"{filePath}\\Suppliers.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);
    while (reader.Read()) // Each Read() call will move to the next row
    {
        // Get the values from the current row...
        string nif = reader.GetString(1);
        string name = reader.GetString(2);
        string phoneNumber = reader.GetString(3);
        string email = reader.GetString(4);
        string address = reader.GetString(6);
        string postalCode = reader.GetString(7);
        string city = reader.GetString(8);
        string countryRegion = reader.GetString(9);
        string market = reader.GetString(0);

        dataTableIdentityDimension.Rows.Add(nif, name, phoneNumber, email, address, postalCode, city, countryRegion, market);
    }
}

// Read the Products Excel file and insert the data into the Product Dimension table
using (var stream = File.Open($"{filePath}\\Products.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);
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

        double purchasingPrice = double.Parse(reader.GetString(9));

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
    while (reader.Read()) // Each Read() call will move to the next row
    {
        // Get the values from the current row...
        int nifClient = int.Parse(reader.GetString(0));
        string number = reader.GetString(4);
        string type = reader.GetString(3);
        string cod = reader.GetString(5);
        int quantity = int.Parse(reader.GetString(7));
        float netAmount = float.Parse(reader.GetString(8));
        float grossAmount = float.Parse(reader.GetString(9));
        float vat = float.Parse(reader.GetString(10));
        string originalCurrency = reader.GetString(11);
        float exchangeRate = float.Parse(reader.GetString(12));
        int netAmountOriginalCurrency = int.Parse(reader.GetString(13));
        int grossAmountOriginalCurrency = int.Parse(reader.GetString(14));
        int vatOriginalCurrency = int.Parse(reader.GetString(15));

        _ = uniqueDates.Add(reader.GetString(2));

        dataTableSalesFactTable.Rows.Add(nifClient, number, type, cod, quantity, netAmount, grossAmount, vat, originalCurrency, exchangeRate, netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency);
    }
}

// Read the Purchase Invoices Excel file and insert the data into the Purchases Fact Table
using (var stream = File.Open($"{filePath}\\PurchaseInvoices.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);
    while (reader.Read()) // Each Read() call will move to the next row
    {
        // Get the values from the current row...
        int nifClient = int.Parse(reader.GetString(0));
        string number = reader.GetString(4);
        string type = reader.GetString(3);
        string cod = reader.GetString(6);
        int quantity = int.Parse(reader.GetString(8));
        string ourRef = reader.GetString(5);
        float netAmount = float.Parse(reader.GetString(9));
        float grossAmount = float.Parse(reader.GetString(10));
        float vat = float.Parse(reader.GetString(11));
        string originalCurrency = reader.GetString(12);
        float exchangeRate = float.Parse(reader.GetString(13));
        int netAmountOriginalCurrency = int.Parse(reader.GetString(14));
        int grossAmountOriginalCurrency = int.Parse(reader.GetString(15));
        int vatOriginalCurrency = int.Parse(reader.GetString(16));

        uniqueDates.Add(reader.GetString(2));

        dataTableSalesFactTable.Rows.Add(nifClient, number, type, cod, quantity, ourRef, netAmount, grossAmount, vat, originalCurrency, exchangeRate, netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency);
    }
}

// Read the Movements Excel file and insert the data into the Movements Fact Table
using (var stream = File.Open($"{filePath}\\StockMovements.xlsx", FileMode.Open, FileAccess.Read))
{
    using var reader = ExcelReaderFactory.CreateReader(stream);
    while (reader.Read()) // Each Read() call will move to the next row
    {
        // Get the values from the current row...
        string cod = reader.GetString(0);
        string doc = reader.GetString(5);
        string type = reader.GetString(4);
        int entryQuantity = int.Parse(reader.GetString(6));
        int exitQuantity = int.Parse(reader.GetString(7));
        decimal entryValue = decimal.Parse(reader.GetString(9));
        decimal exitValue = decimal.Parse(reader.GetString(10));
        decimal movementValue = decimal.Parse(reader.GetString(11));
        string thirdParty = reader.GetString(14);

        uniqueDates.Add(reader.GetString(2));

        // For each unique cod, keep track of the entryQuantity and exitQuantity values so we can get the existing stock for each product
        if (!productColumns.TryGetValue(cod, out IList<string>? value))
        {
            productColumns.Add(cod, new List<string> { reader.GetString(8) });
        }
        else
        {
            int existingQuantity = int.Parse(value[0]);
            existingQuantity += int.Parse(reader.GetString(8));
            value[0] = existingQuantity.ToString();
        }

        dataTableMovementsFactTable.Rows.Add(cod, doc, type, entryQuantity, exitQuantity, entryValue, exitValue, movementValue, thirdParty);
    }
}

// For each cod in productColumns, extract the value and search for the corresponding column to extract the existing stock of each product and add it to a new column "ExistingStock"
for (int i = 0; i < productColumns["Cod"].Count; i++)
{
    string cod = productColumns["Cod"][i];

    // Get the existing stock for the product
    int existingStock = int.Parse(productColumns[cod][0]);

    if (!productColumns.ContainsKey("ExistingStock"))
        productColumns.Add("ExistingStock", new List<string>());
    productColumns["ExistingStock"].Add(existingStock.ToString());
}

// For each column in the productColumns dictionary, add the values to the Product Dimension table
for (int i = 0; i < productColumns["Cod"].Count; i++)
{
    dataTableProductDimension.Rows.Add(productColumns["Cod"][i], productColumns["Family"][i], productColumns["Description"][i], decimal.Parse(productColumns["PercentageOfVat"][i]), int.Parse(productColumns["ExistingStock"][i]), float.Parse(productColumns["PurchasingPrice"][i]), float.Parse(productColumns["StockValue"][i]));
}

for (int i = 0; i < uniqueDates.Count; i++)
{
    string date = uniqueDates.ElementAt(i);

    // Get the day, month, and year from the date
    string[] dateParts = date.Split('-');

    // 20240101 - 01 - 01 - 2024
    date = dateParts[2] + dateParts[1] + dateParts[0];
    int day = int.Parse(dateParts[0]);
    int month = int.Parse(dateParts[1]);
    int year = int.Parse(dateParts[2]);

    dataTableDateDimension.Rows.Add(date, day, month, year);
}

// SqlBulkCopy for the Date Dimension table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.Date_Dimension";
    bulkCopy.WriteToServer(dataTableDateDimension);
}

// SqlBulkCopy for the Identity Dimension table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.Identity_Dimension";
    bulkCopy.WriteToServer(dataTableIdentityDimension);
}

// SqlBulkCopy for the Invoice Dimension table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.Invoice_Dimension";
    bulkCopy.WriteToServer(dataTableInvoiceDimension);
}

// SqlBulkCopy for the Product Dimension table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.Product_Dimension";
    bulkCopy.WriteToServer(dataTableProductDimension);
}

// SqlBulkCopy for the Sales Fact Table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.SalesFactTable";
    bulkCopy.WriteToServer(dataTableSalesFactTable);
}

// SqlBulkCopy for the Purchases Fact Table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.PurchasesFactTable";
    bulkCopy.WriteToServer(dataTablePurchasesFactTable);
}

// SqlBulkCopy for the Movements Fact Table
using (SqlBulkCopy bulkCopy = new(connection))
{
    bulkCopy.DestinationTableName = "dbo.MovementsFactTable";
    bulkCopy.WriteToServer(dataTableMovementsFactTable);
}

// Finally, close the connection
connection.Close();