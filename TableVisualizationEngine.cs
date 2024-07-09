using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ConsoleTableExt;

namespace drinks_info
{
    public class TableVisualizationEngine
    {
        //4 passo - criar a classe, e o método responsável por mostrar os dados no console
    
        //[AllowNull] check, allows to have the name as null, so it dosent run into null exceptions
        public static void ShowTable<T>(List<T> tableData, [AllowNull] string tableName) where T : class
        {
            Console.Clear();

            if (tableName == null)
                tableName = "";

            Console.WriteLine("\n\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithColumn(tableName)
                .ExportAndWriteLine();
            Console.WriteLine("\n\n");

        }
   
    }   

}