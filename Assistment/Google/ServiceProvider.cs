using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google;
using Google.Apis;
using Google.Apis.Customsearch;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;

namespace Assistment.Google
{
    public static class ServiceProvider
    {
        public static string SearchID { get; private set; }
        public static BaseClientService.Initializer Initializer { get; private set; }

        static ServiceProvider()
        {
            KeyGenerator KeyGen = new KeyGenerator();
            Initializer = new BaseClientService.Initializer { ApiKey = KeyGen.ApiKey };
            SearchID = KeyGen.SearchID;
        }


        public static CustomsearchService GetSearchService()
        {
            CustomsearchService css = new CustomsearchService(Initializer);
            return css;
        }

        public static CseResource.ListRequest SearchImages(this CustomsearchService Service, string QueryText)
        {
            CseResource.ListRequest List = Service.Cse.List( QueryText);
            List.Cx = SearchID;
            List.Filter = CseResource.ListRequest.FilterEnum.Value1;
            List.Safe = CseResource.ListRequest.SafeEnum.Off;
            List.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            List.Start = 1;
            List.Num = 10;
            return List;
        }
    }
}
