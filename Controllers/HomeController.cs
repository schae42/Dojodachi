using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
    
        //============================================
        //  Root Route
        //============================================
    
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetObjectFromJson<Dachi>("DachiData") == null)
            {
                HttpContext.Session.SetObjectAsJson("DachiData", new Dachi());
            }            
            ViewBag.DachiData = HttpContext.Session.GetObjectFromJson<Dachi>("DachiData");
                if(ViewBag.DachiData.fullness < 1 || ViewBag.DachiData.happiness < 1 ){
                    ViewBag.DachiData.status = "Your Dachi just died...";
                } 
                if(ViewBag.DachiData.fullness > 100 && ViewBag.DachiData.happiness > 100 && ViewBag.DachiData.energy > 100){
                    ViewBag.DachiData.status = "Your Dachi just succeeded in life...";
                }

            return View();
        }

        //============================================
        //  RESET Route
        //============================================

        [HttpGet]
        [Route("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        //============================================
        //  FEED Route
        //============================================

        [HttpGet]
        [Route("feed")]
        public IActionResult FeedDachi()
        {
            Dachi CurrDachiData = HttpContext.Session.GetObjectFromJson<Dachi>("DachiData");
            if(CurrDachiData.meals > 0){
                CurrDachiData.feed();
            } else {
                CurrDachiData.status = "No Meals... your Dachi cannot eat. Work to earn meals.";
            }
            HttpContext.Session.SetObjectAsJson("DachiData",CurrDachiData);
            return RedirectToAction("Index");
        }

        //============================================
        //  PLAY Route
        //============================================
        [HttpGet]
        [Route("sleep")]
        public IActionResult SleepDachi()
        {
            Dachi CurrDachiData = HttpContext.Session.GetObjectFromJson<Dachi>("DachiData");
            CurrDachiData.sleep();
            HttpContext.Session.SetObjectAsJson("DachiData",CurrDachiData);
            return RedirectToAction("Index");
        }
        //============================================
        //  WORK Route
        //============================================

        [HttpGet]
        [Route("play")]
        public IActionResult PlayDachi()
        {
            Dachi CurrDachiData = HttpContext.Session.GetObjectFromJson<Dachi>("DachiData");
                if(CurrDachiData.energy > 0){
                CurrDachiData.play();
            } else {
                CurrDachiData.status = "No Energy... your Dachi cannot work or play. Dachi needs sleep.";
            }
            HttpContext.Session.SetObjectAsJson("DachiData",CurrDachiData);
            return RedirectToAction("Index");
        }
        //============================================
        //  SLEEP Route
        //============================================
        
        [HttpGet]
        [Route("work")]
        public IActionResult WorkDachi()
        {
            Dachi CurrDachiData = HttpContext.Session.GetObjectFromJson<Dachi>("DachiData");
                if(CurrDachiData.energy > 0){
                CurrDachiData.work();
            } else {
                CurrDachiData.status = "No Energy... your Dachi cannot work or play. Dachi needs sleep.";
            }
            HttpContext.Session.SetObjectAsJson("DachiData",CurrDachiData);
            return RedirectToAction("Index");
        }


    }

    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes theobject to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upone retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}