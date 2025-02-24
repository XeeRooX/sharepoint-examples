using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerJobExample.Sharepoint
{
    class SPListProcessing
    {
        /// <summary>
        /// Удаляет элементы списка SharePoint.
        /// </summary>
        /// <param name="listItems">Перечисление элементов списка SharePoint, которые необходимо удалить</param>
        /// <param name="web">Сайт SharePoint, на котором расположен список</param>
        internal static void BatchDeleteItems(IEnumerable<SPListItem> listItems, SPWeb web)
        {
            StringBuilder batchString = new StringBuilder();
            batchString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");

            foreach (SPListItem item in listItems)
            {
                batchString.Append("<Method>");
                batchString.Append("<SetList Scope=\"Request\">" + Convert.ToString(item.ParentList.ID) + "</SetList>");
                batchString.Append("<SetVar Name=\"ID\">" + Convert.ToString(item.ID) + "</SetVar>");
                batchString.Append("<SetVar Name=\"Cmd\">Delete</SetVar>");
                batchString.Append("</Method>");
            }

            batchString.Append("</Batch>");
            web.ProcessBatchData(batchString.ToString());
        }

        internal static void CleanList(SPList list, SPWeb web)
        {
            var listGuid = list.ID.ToString();

            StringBuilder batchString = new StringBuilder();
            batchString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Batch>");

            foreach (SPListItem item in list.Items)
            {
                batchString.Append("<Method>");
                batchString.Append($"<SetList Scope=\"Request\">{listGuid}</SetList>");
                batchString.Append("<SetVar Name=\"ID\">" + Convert.ToString(item.ID) + "</SetVar>");
                batchString.Append("<SetVar Name=\"Cmd\">Delete</SetVar>");
                batchString.Append("</Method>");
            }

            batchString.Append("</Batch>");
            web.ProcessBatchData(batchString.ToString());
        }

        /// <summary>
        /// Возвращает список элементов списка SharePoint из постраничной выборки.
        /// </summary>
        /// <param name="list">Список SharePoint</param>
        /// <param name="query">Caml запрос к списку SharePoint</param>
        /// <returns>Список элементов списка SharePoint</returns>
        internal static List<SPListItem> GetSPListItems(SPList list, SPQuery query)
        {
            SPListItemCollection itemCollection;
            List<SPListItem> listItems = new List<SPListItem>();
            int itemCount = 2000;
            int currentCount = itemCount;
            string lastItemIdOnPage = null;
            query.RowLimit = (uint)itemCount;

            do
            {
                if (lastItemIdOnPage != null)
                {
                    SPListItemCollectionPosition position = new SPListItemCollectionPosition(lastItemIdOnPage);
                    query.ListItemCollectionPosition = position;
                }

                itemCollection = list.GetItems(query);
                lastItemIdOnPage = itemCollection?.ListItemCollectionPosition?.PagingInfo;
                currentCount = itemCollection.Count;

                listItems.AddRange(itemCollection.Cast<SPListItem>());
            }
            while (currentCount == itemCount && lastItemIdOnPage != null);

            return listItems;
        }

    }
}
