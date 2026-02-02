using BankAPI.DTO.ClientDTO;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsControllers : ControllerBase
{
    private readonly ILogger<ClientsControllers> _logger;
    private readonly IClientService _clientService;

    public ClientsControllers(
        ILogger<ClientsControllers> logger, 
        IClientService clientService)
    {
        _logger = logger;
        _clientService = clientService;
    }

    //Get Client by ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientResponseDTO>> GetClientById(int id)
    {
        _logger.LogInformation("Get client {ClientId}", id);

        var client = await _clientService.GetClientByIdAsync(id);
        
        if (client is null)
        {
            return NotFound($"Client {id} not found");
        }
        
        return Ok(client);
    }
    
    //Create new Client
    [HttpPost]
    public async Task<ActionResult<ClientResponseDTO>> CreateClient(ClientCreateDTO dto)
    {
        var client = await _clientService.CreateClientAsync(dto);
        
        return CreatedAtAction(nameof(GetClientById), 
            new { id = client.Id }, 
            client
            );
    }
    
    //Get all clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponseDTO>>> GetAllClientsAsync()
    {
        _logger.LogInformation("Get All Clients");
        
        var clients = await _clientService.GetAllClientsAsync();

        if (!clients.Any())
        {
            return NotFound();
        }
        
        return Ok(clients);
    }

    //Get Client by name
    [HttpGet("{name}")]
    public async Task<ActionResult<ClientResponseDTO>> GetClientByName(string name)
    {
        _logger.LogInformation("Get client by name {name}", name);

        var client = await _clientService.GetClientByNameAsync(name);

        if (client is null)
        {
            return NotFound($"Client {name} not found");
        }

        return Ok(client);
    }

    //Delete client
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        _logger.LogInformation("Delete client with id {id}", id);
        
        var client = await _clientService.GetClientByIdAsync(id);

        if (client == null)
        {
            return NotFound($"Client with id {id} not found");
        }
        
        await _clientService.DeleteClientAsync(id);
        
        return Ok("Client deleted");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateClient(
        [FromRoute] int id, 
        [FromBody] ClientUpdateDTO dto)
    {
        _logger.LogInformation("Update client with id {id}", id);
        
        var client = await _clientService.UpdateClientAsync(id, dto);

        if (client is null)
        {
            return NotFound($"Client {id} not found");
        }
        
        return Ok(client);
    }

    [HttpPost("{id:int}/status")]
    public async Task<IActionResult> ClientUpdateStatus(int id, ClientStatusDTO dto)
    {
        var result = await _clientService.ClientUpdateStatusAsync(id, dto.Status);

        if (!result)
        {
            return NotFound($"Client with id {id} not found");
        }
        
        return NoContent();
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ClientResponseDTO>>> GetAllActiveClients()
    {
        _logger.LogInformation("GetAllActiveClients");

        var result = await _clientService.GetActiveAclientsAsync();
        
        return Ok(result);
    }
}