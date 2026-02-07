using BankAPI.DTO.ClientDTO;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsControllers : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsControllers(
        IClientService clientService)
    {
        _clientService = clientService;
    }

    //Get Client by ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClientResponseDTO>> GetClientById(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        
        if (client is null)
        {
            return NotFound();
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
        var client = await _clientService.GetClientByNameAsync(name);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    //Delete client
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);

        if (client == null)
        {
            return NotFound();
        }
        
        await _clientService.DeleteClientAsync(id);
        
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateClient(
        [FromRoute] int id, 
        [FromBody] ClientUpdateDTO dto)
    {
        var client = await _clientService.UpdateClientAsync(id, dto);

        if (client is null)
        {
            return NotFound();
        }
        
        return Ok(client);
    }

    [HttpPost("{id:int}/status")]
    public async Task<IActionResult> ClientUpdateStatus(int id, ClientStatusDTO dto)
    {
        var result = await _clientService.ClientUpdateStatusAsync(id, dto.Status);

        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ClientResponseDTO>>> GetAllActiveClients()
    {
        var result = await _clientService.GetActiveСlientsAsync();
        
        return Ok(result);
    }
}