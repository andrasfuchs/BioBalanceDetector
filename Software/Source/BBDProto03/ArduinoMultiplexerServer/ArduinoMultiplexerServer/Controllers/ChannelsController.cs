using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ArduinoMultiplexerServer.Controllers
{
    [EnableCors(origins: "http://localhost:56417", headers: "*", methods: "*")]
    [RoutePrefix("api/channels")]
    public class ChannelsController : ApiController
    {
        [Route("test")]
        [HttpGet]
        public string Test()
        {
            return "I'm ok, thanks!";
        }


        [Route("listsessions/")]
        [HttpGet]
        public string[] ListSessionIds()
        {
            return SessionsCache.ListSessionIds();
        }

        [Route("createsession")]
        [HttpGet]
        public string CreateSession()
        {
            return SessionsCache.GetSession("").SessionId;
        }

        [Route("list/{sessionId}")]
        [HttpGet]
        public List<ADCChannel> GetADCChannelList(string sessionId)
        {            
            return SessionsCache.GetSession(sessionId).ChannelList;
        }

        [Route("appendvalue/{sessionId}/{channelIndex}/{timeStamp}/{value}")]
        [HttpGet]
        public void AppendChannelValue(string sessionId, byte channelIndex, DateTime timeStamp, double value)
        {
            SessionsCache.GetSession(sessionId).ChannelList[channelIndex].AppendValue(timeStamp, value);
        }

        [Route("getvalues/{sessionId}/{channelIndex}/{timeSpan}")]
        [HttpGet]
        public ADCChannelValue[] GetChannelValues(string sessionId, byte channelIndex, TimeSpan timeSpan)
        {
            return SessionsCache.GetSession(sessionId).ChannelList[channelIndex].GetValues(DateTime.Now - timeSpan);
        }

    }
}
