using inRiver.Remoting;
using inRiver.Remoting.Objects;
using inRiver.Remoting.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace API_Basics
{
    class ApiBasicConsoleApp
    {
        static RemoteManager remoteManager;
        private const String PRODUCT_NUMBER = "x123456789";
        private const string ITEM_NUMBER = "hej1234";
        private static Entity _product;
        private static Entity _item;
        private static Entity _resource;
        private static CVL _cvl;

        static void Main(string[] args)
        {
            #region 1. Setup

            remoteManager = InitRemoteManager();

            #endregion

            #region 2. Create Entities and Link

            CreateProduct();
            CreateItem();
            // CreateResource();
            LinkProductToItem();
            //LinkProductToResource();

            #endregion

            #region 3. Create CVL

            CreateCVL();

            #endregion

            #region 4. Search

            // SearchByUniqueNumber();
            // SearchByCriteria();
            // SearchByLinkQuery();
            // SearchByComplexQuery();
            #endregion

            // Delete();
        }

        private static RemoteManager InitRemoteManager()
        {
            try
            {
                RemoteManager manager = RemoteManager.CreateInstance(
                    "https://demo.remoting.productmarketingcloud.com",
                    "academy62@inriver.com",
                    "inRiverBest4Ever!"
                    );

                return manager;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        #region 2. Create Entities
        private static void CreateProduct()
        {
            EntityType productEntityType = remoteManager.ModelService.GetEntityType("Product");

            Entity productEntity = Entity.CreateEntity(productEntityType);

            Field productNumber = productEntity.GetField("ProductNumber");
            productNumber.Data = PRODUCT_NUMBER;

            _product = remoteManager.DataService.GetEntityByUniqueValue("ProductNumber", PRODUCT_NUMBER, LoadLevel.DataAndLinks);

            if( _product == null)
            {
                _product = remoteManager.DataService.AddEntity(productEntity);
            }
        }

        private static void CreateItem()
        {
            EntityType itemType = remoteManager.ModelService.GetEntityType("Item");

            Entity productItem = Entity.CreateEntity(itemType);
            Field itemNumber = productItem.GetField("itemNumber");
            itemNumber.Data = ITEM_NUMBER;

            _item = remoteManager.DataService.GetEntityByUniqueValue("itemNumber", itemNumber.Data.ToString(), LoadLevel.DataAndLinks);
            if(_item == null)
            {
                _item = remoteManager.DataService.AddEntity(productItem);
            }
         
        }

        private static void CreateResource()
        {
            
        }

        private static void LinkProductToItem()
        {
            Link productToItemLink = new Link();
            List<LinkType> linkTypes = remoteManager.ModelService.GetLinkTypesForEntityType(_product.EntityType.Id);
            LinkType productToItemLinkType = linkTypes.Find(l => l.TargetEntityTypeId == _item.EntityType.Id);
            productToItemLink.LinkType = productToItemLinkType;
            productToItemLink.Source = _product;
            productToItemLink.Target = _item;

            if (!remoteManager.DataService.LinkAlreadyExists(_product.Id, _item.Id, null, "ProductItem"))
            {
                remoteManager.DataService.AddLinkLast(productToItemLink);
            }
            
        }

        private static void LinkProductToResource()
        {
            
        }

        #endregion

        #region 3. Create CVL

        private static void CreateCVL()
        {

            CVL newCVLAsLocaleString = remoteManager.ModelService.GetCVL("NewCVLAsLocaleStringId");
            if(newCVLAsLocaleString == null)
            {
                newCVLAsLocaleString = new CVL
                {
                    DataType = DataType.LocaleString,
                    Id = "NewCVLAsLocaleStringId"
                };

                remoteManager.ModelService.AddCVL(newCVLAsLocaleString);
            }
            
            AddCvlValue(newCVLAsLocaleString.Id, "key", "yay");
        }
        private static void AddCvlValue(string cvlId, string newCVLValueKey, string newCVLValue)
        {
            CVL cvl = remoteManager.ModelService.GetCVL(cvlId);
            bool newValue = false;
            CVLValue cvlVal = remoteManager.ModelService.GetCVLValueByKey(newCVLValueKey, cvlId);

            if(cvlVal == null)
            {
                newValue = true;
                cvlVal = new CVLValue() { CVLId = cvlId, Key = newCVLValueKey };
            }
            

            if (cvl.DataType == DataType.String)
            {
                cvlVal.Value = newCVLValue;
            }
            else if (cvl.DataType == DataType.LocaleString)
            {
                LocaleString locale = new LocaleString(remoteManager.UtilityService.GetAllLanguages());
                CultureInfo cultureInfo = new CultureInfo("en");
                locale[cultureInfo] = newCVLValue;
                locale[new CultureInfo("sv")] = newCVLValue + "_SV";
                cvlVal.Value = locale;
            }

            if(newValue)
            {
                remoteManager.ModelService.AddCVLValue(cvlVal);
            } else
            {
                remoteManager.ModelService.UpdateCVLValue(cvlVal);
            }
        }

        #endregion

        #region 4. Search

        private static void SearchByUniqueNumber()
        {
            Entity found = remoteManager.DataService.GetEntityByUniqueValue("ProductNumber", PRODUCT_NUMBER, LoadLevel.Shallow);
        }

        private static void SearchByCriteria()
        {
            Criteria itemCriteria = new Criteria
            {
                FieldTypeId = "ItemNumber",
                Operator = Operator.BeginsWith,
                Value = ITEM_NUMBER
            };

            List<Entity> entities = remoteManager.DataService.Search(itemCriteria, LoadLevel.Shallow);
        }

        private static void SearchByLinkQuery()
        {

        }

        private static void SearchByComplexQuery()
        {


        }
        #endregion

        #region Delete
        private static void Delete()
        {

        }
        #endregion

    }
}
