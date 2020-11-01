using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ControlServer.Data;
using ControlServer.Models;
using ControlServer.Services.Ads;

namespace ControlServer.Controllers
{
    [ApiController]
    [Route("api/lights")]
    public class LightController : ControllerBase
    {
        private readonly ILogger<LightController> logger;

        private readonly IAdsService adsService;

        public LightController(ILogger<LightController> logger, IAdsService adsService)
        {
            this.logger = logger;
            this.adsService = adsService;
        }

        [HttpGet]
        public List<LightModel> GetLights()
        {
            return DataContext.Lights.Select((l, i) => new LightModel() { Id = i, Name = l.Name }).ToList();
        }

        [HttpGet("{id}/power")]
        public async Task<ActionResult<LightPowerModel>> GetPowerAsync(int id)
        {
            if (id >= 0 && id < DataContext.Lights.Length)
            {
                Light light = DataContext.Lights[id];
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
            if (id >= 0 && id < DataContext.Lights.Length)
            {
                Light light = DataContext.Lights[id];
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
