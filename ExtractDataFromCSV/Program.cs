// Workflow:
// Connect to SQL Server database
// Create a data table for bulk inserts
// Read a CSV file line by line so we don't load the entire file into memory
// Insert each line into the data table
// Bulk insert the data table into the database
// Close the connection

using ExcelDataReader;
using System.Reflection;

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

// Finally, close the connection
connection.Close();