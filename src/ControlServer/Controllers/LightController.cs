using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ControlServer.Data;
using ControlServer.Models;
using ControlServer.Services.Ads;
using ControlServer.Services.Data;

namespace ControlServer.Controllers
{
    [ApiController]
    [Route("api/lights")]
    public class LightController : ControllerBase
    {
        private readonly ILogger<LightController> logger;

        private readonly IDataService dataService;

        private readonly IAdsService adsService;

        public LightController(ILogger<LightController> logger, IDataService dataService, IAdsService adsService)
        {
            this.logger = logger;
            this.dataService = dataService;
            this.adsService = adsService;
        }

        [HttpGet]
        public List<LightModel> GetLights()
        {
            return this.dataService.Lights.Select((l, i) => new LightModel() { Id = i, Name = l.Name, Icon = l.Icon }).ToList();
        }

        [HttpGet("{id}/power")]
        public async Task<ActionResult<LightPowerModel>> GetPowerAsync(int id)
        {
            if (id >= 0 && id < dataService.Lights.Length)
            {
                Light light = dataService.Lights[id];
                bool result = await this.adsService.ReadBoolAsync(light.ReadGroup, light.ReadOffset);
                return new LightPowerModel()
                {
                    Value = result
                };
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPut("{id}/power")]
        public async Task<ActionResult<LightPowerModel>> PutPowerAsync(int id, LightPowerModel value)
        {
            if (id >= 0 && id < dataService.Lights.Length)
            {
                Light light = dataService.Lights[id];
                bool result = await this.adsService.ReadBoolAsync(light.ReadGroup, light.ReadOffset);
                if (value.Value != result)
                {
                    // toggle
                    await this.adsService.WriteBoolAsync(light.TriggerGroup, light.TriggerOffset, true);
                    await this.adsService.WriteBoolAsync(light.TriggerGroup, light.TriggerOffset, false);
                }

                return value;
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
