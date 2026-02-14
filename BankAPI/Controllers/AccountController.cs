using BankAPI.DTO.AccountDTO;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/accounts")]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(
        IAccountService accountService
        )
    {
        _accountService = accountService;
    }

    [HttpGet("id:int")]
    public async Task<ActionResult<AccountResponseDto>> GetById(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        if (account is null)
        {
            return NotFound();
        }
        
        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponseDto>> CreateAccount(AccountCreateDto accountCreateDto)
    {
        var account = await _accountService.CreateAccount(accountCreateDto);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = account.ClientId }, 
            account
            );
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAllAccountsByClientId(int clientId)
    {
        var accounts = await _accountService.GetAllAccountsByClientIdAsync(clientId);
        
        return Ok(accounts);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<AccountResponseDto>> AccountUpdateStatusAsync(int id,
        AccountUpdateDto accountUpdateDto)
    {
        var updateAccount = await _accountService.AccountUpdateStatusAsync(id, accountUpdateDto);

        if (updateAccount is null)
        {
            return NotFound();
        }
        
        return Ok(updateAccount);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<bool>> CloseAccountAsync(int id)
    {
        var closeAccount = await _accountService.CloseAccountAsync(id);

        return Ok(closeAccount);
    }

    [HttpPatch("{id:int}/plan")]
    public async Task<ActionResult<AccountResponseDto>> UpdatePlanAsync(int id, 
        AccountUpdateDto accountUpdateDto)
    {
        var updateAccountPlan = await _accountService.AccountUpdatePlanAsync(id, accountUpdateDto);

        if (updateAccountPlan is null)
        {
            return NotFound();
        }
        return Ok(updateAccountPlan);
    }
}