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
dataTableDateDimension.Columns.Add("Date_Key", typeof(int));
dataTableDateDimension.Columns.Add("Day", typeof(int));
dataTableDateDimension.Columns.Add("Date", typeof(DateTime));
dataTableDateDimension.Columns.Add("Month", typeof(int));
dataTableDateDimension.Columns.Add("Year", typeof(int));

// Create the columns for the Identity Dimension table
dataTableIdentityDimension.Columns.Add("NIF", typeof(int));
dataTableIdentityDimension.Columns.Add("Name", typeof(string));
dataTableIdentityDimension.Columns.Add("Country_Region", typeof(string));
dataTableIdentityDimension.Columns.Add("Market", typeof(string));

// Create the columns for the Invoice Dimension table
dataTableInvoiceDimension.Columns.Add("Number", typeof(string));
dataTableInvoiceDimension.Columns.Add("Type", typeof(string));
dataTableInvoiceDimension.Columns.Add("Date_Key", typeof(int));

// Create the columns for the Product Dimension table
dataTableProductDimension.Columns.Add("Cod", typeof(string));
dataTableProductDimension.Columns.Add("Name", typeof(string));
dataTableProductDimension.Columns.Add("Description", typeof(string));
dataTableProductDimension.Columns.Add("VAT", typeof(float));
dataTableProductDimension.Columns.Add("QtExistencias", typeof(float));
dataTableProductDimension.Columns.Add("ValorExistencias", typeof(float));
dataTableProductDimension.Columns.Add("PCM", typeof(float));

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
dataTableMovementsFactTable.Columns.Add("MovementDate", typeof(DateTime));
dataTableMovementsFactTable.Columns.Add("DocumentDate", typeof(DateTime));
dataTableMovementsFactTable.Columns.Add("EntryQuantity", typeof(int));
dataTableMovementsFactTable.Columns.Add("ExitQuantity", typeof(int));
dataTableMovementsFactTable.Columns.Add("EntryValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("ExitValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("MovementValue", typeof(decimal));
dataTableMovementsFactTable.Columns.Add("ThirdParty", typeof(string));

// Read the files in ..\Data
string? directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

if (directory == null)
{
    throw new DirectoryNotFoundException("Could not find the directory.");
}

string filePath = Path.Combine(directory, "..\\Data");

// Read the Clients and Suppliers CSV files and insert the data into the Identity Dimension table
using (var stream = File.Open($"{filePath}\\Clients.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            string nif = reader.GetString(0);
            string name = reader.GetString(1);
            string countryRegion = reader.GetString(2);
            string market = reader.GetString(3);

            dataTableIdentityDimension.Rows.Add(nif, name, countryRegion, market);
        }
    }
}

// Read the Suppliers Excel file and insert the data into the Identity Dimension table
using (var stream = File.Open($"{filePath}\\Suppliers.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            string nif = reader.GetString(0);
            string name = reader.GetString(1);
            string countryRegion = reader.GetString(2);
            string market = reader.GetString(3);

            dataTableIdentityDimension.Rows.Add(nif, name, countryRegion, market);
        }
    }
}

// Read the Invoices Excel file and insert the data into the Invoice Dimension table
using (var stream = File.Open($"{filePath}\\Invoices.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            string number = reader.GetString(0);
            string type = reader.GetString(1);
            int dateKey = int.Parse(reader.GetString(2));

            dataTableInvoiceDimension.Rows.Add(number, type, dateKey);
        }
    }
}

// Read the Products Excel file and insert the data into the Product Dimension table
using (var stream = File.Open($"{filePath}\\Products.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            string cod = reader.GetString(0);
            string name = reader.GetString(1);
            string description = reader.GetString(2);
            float vat = float.Parse(reader.GetString(3));
            float qtExistencias = float.Parse(reader.GetString(4));
            float valorExistencias = float.Parse(reader.GetString(5));
            float pcm = float.Parse(reader.GetString(6));

            dataTableProductDimension.Rows.Add(cod, name, description, vat, qtExistencias, valorExistencias, pcm);
        }
    }
}

// Read the Sales Invoices Excel file and insert the data into the Sales Fact Table
using (var stream = File.Open($"{filePath}\\SalesInvoices.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            int nifClient = int.Parse(reader.GetString(0));
            string number = reader.GetString(1);
            string type = reader.GetString(2);
            string cod = reader.GetString(3);
            int quantity = int.Parse(reader.GetString(4));
            float netAmount = float.Parse(reader.GetString(5));
            float grossAmount = float.Parse(reader.GetString(6));
            float vat = float.Parse(reader.GetString(7));
            string originalCurrency = reader.GetString(8);
            float exchangeRate = float.Parse(reader.GetString(9));
            int netAmountOriginalCurrency = int.Parse(reader.GetString(10));
            int grossAmountOriginalCurrency = int.Parse(reader.GetString(11));
            int vatOriginalCurrency = int.Parse(reader.GetString(12));

            dataTableSalesFactTable.Rows.Add(nifClient, number, type, cod, quantity, netAmount, grossAmount, vat, originalCurrency, exchangeRate, netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency);
        }
    }
}

// Read the Purchase Invoices Excel file and insert the data into the Purchases Fact Table
using (var stream = File.Open($"{filePath}\\PurchaseInvoices.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            int nifClient = int.Parse(reader.GetString(0));
            string number = reader.GetString(1);
            string type = reader.GetString(2);
            string cod = reader.GetString(3);
            int quantity = int.Parse(reader.GetString(4));
            string ourRef = reader.GetString(5);
            float netAmount = float.Parse(reader.GetString(6));
            float grossAmount = float.Parse(reader.GetString(7));
            float vat = float.Parse(reader.GetString(8));
            string originalCurrency = reader.GetString(9);
            float exchangeRate = float.Parse(reader.GetString(10));
            int netAmountOriginalCurrency = int.Parse(reader.GetString(11));
            int grossAmountOriginalCurrency = int.Parse(reader.GetString(12));
            int vatOriginalCurrency = int.Parse(reader.GetString(13));

            dataTableSalesFactTable.Rows.Add(nifClient, number, type, cod, quantity, ourRef, netAmount, grossAmount, vat, originalCurrency, exchangeRate, netAmountOriginalCurrency, grossAmountOriginalCurrency, vatOriginalCurrency);
        }
    }
}

// Read the Movements Excel file and insert the data into the Movements Fact Table
using (var stream = File.Open($"{filePath}\\StockMovements.xlsx", FileMode.Open, FileAccess.Read))
{
    using (var reader = ExcelReaderFactory.CreateReader(stream))
    {
        while (reader.Read()) // Each Read() call will move to the next row
        {
            // Get the values from the current row...
            string cod = reader.GetString(0);
            string doc = reader.GetString(1);
            string type = reader.GetString(2);
            DateTime movementDate = DateTime.Parse(reader.GetString(3), new CultureInfo("pt-PT"));
            DateTime documentDate = DateTime.Parse(reader.GetString(4), new CultureInfo("pt-PT"));
            int entryQuantity = int.Parse(reader.GetString(5));
            int exitQuantity = int.Parse(reader.GetString(6));
            decimal entryValue = decimal.Parse(reader.GetString(7));
            decimal exitValue = decimal.Parse(reader.GetString(8));
            decimal movementValue = decimal.Parse(reader.GetString(9));
            string thirdParty = reader.GetString(10);

            dataTableMovementsFactTable.Rows.Add(cod, doc, type, movementDate, documentDate, entryQuantity, exitQuantity, entryValue, exitValue, movementValue, thirdParty);
        }
    }
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