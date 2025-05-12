using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;

/// <summary>
/// Excel dosyalarını okumak için yardımcı sınıf
/// </summary>
public class ExcelHelper
{
    /// <summary>
    /// Excel dosyasını okuyarak DataTable'a dönüştürür
    /// </summary>
    /// <param name="filePath">Excel dosyasının tam yolu</param>
    /// <returns>DataTable</returns>
    public static DataTable ReadExcelFile(string filePath)
    {
        DataTable dt = new DataTable();
        
        try
        {
            // Excel dosyasını oku
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Çalışma sayfası kontrolü
                if (package.Workbook.Worksheets.Count == 0)
                {
                    throw new Exception("Excel dosyasında çalışma sayfası bulunamadı.");
                }
                
                // İlk çalışma sayfasını al (1-based index)
                var worksheet = package.Workbook.Worksheets[1]; // 1-based indexing
                
                if (worksheet == null)
                {
                    throw new Exception("Excel dosyasında çalışma sayfası bulunamadı.");
                }
                
                // Satır ve sütun sayısını al
                if (worksheet.Dimension == null)
                {
                    throw new Exception("Excel dosyası boş veya geçersiz.");
                }
                
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                
                if (rowCount <= 1)
                {
                    throw new Exception("Excel dosyasında veri satırı bulunamadı.");
                }
                
                // Başlık satırını oku ve kolonları oluştur
                for (int col = 1; col <= colCount; col++)
                {
                    string headerText = worksheet.Cells[1, col].Text.Trim();
                    if (!string.IsNullOrEmpty(headerText))
                    {
                        dt.Columns.Add(headerText);
                    }
                }
                
                // Veri satırlarını oku
                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    bool rowHasData = false;
                    
                    for (int col = 1; col <= dt.Columns.Count; col++)
                    {
                        string cellValue = worksheet.Cells[row, col].Text;
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            rowHasData = true;
                        }
                        
                        if (col <= dt.Columns.Count)
                        {
                            dataRow[col - 1] = cellValue;
                        }
                    }
                    
                    // Satırda veri varsa ekle
                    if (rowHasData)
                    {
                        dt.Rows.Add(dataRow);
                    }
                }
            }
            
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception("Excel dosyası okunurken bir hata oluştu: " + ex.Message);
        }
    }
}
