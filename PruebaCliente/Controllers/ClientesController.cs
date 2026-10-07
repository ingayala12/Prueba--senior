using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaCliente.Datos;
using PruebaCliente.Modelos;
using PruebaCliente.Modelos.DTOs;

namespace PruebaCliente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;

        public ClientesController(IMapper mapper,ApplicationDbContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var clientes = await db.Clientes.ToListAsync();
            var clientesDTOs = mapper.Map<IEnumerable<ClienteDTOs>>(clientes);
            return Ok(clientesDTOs);
        }

        [HttpGet("{id:int}", Name = "ObtenerPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerPorId(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var cliente = await db.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (cliente is null)
            {
                return NotFound("Cliente No Encontrado");
            }
            var clienteDTOs = mapper.Map<ClienteDTOs>(cliente);
            return Ok(clienteDTOs);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CrearCliente(ClienteCrearDTOs clienteCrearDTOs)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var clienteDTOs = mapper.Map<Cliente>(clienteCrearDTOs);

            await db.Clientes.AddAsync(clienteDTOs);

            return CreatedAtRoute("ObtenerPorId", new { id = clienteDTOs.Id }, clienteDTOs);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActualizarCliente(int id, ClienteDTOs clienteDTOs)
        {
            if (id != clienteDTOs.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = await db.Clientes.FirstOrDefaultAsync(x => x.Id == clienteDTOs.Id);

            if (cliente is null)
            {
                return NotFound("Cliente No Encontrado");
            }

            db.Clientes.Update(cliente);
            return NoContent();

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            if (id == 0)
            {
                return BadRequest(ModelState);
            }
            var cliente = await db.Clientes.FindAsync(id);

            if (cliente is null)
            {
                return BadRequest(ModelState);
            }
            db.Clientes.Remove(cliente);
            return NoContent();

        }
    }
}
