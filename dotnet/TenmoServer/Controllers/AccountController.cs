﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {
        private readonly IAccountSqlDao accountDao;
        private readonly IUserDao userDao;
        public AccountController(IAccountSqlDao _accountDao, IUserDao _userDao)
        {
            accountDao = _accountDao;
            userDao = _userDao;
        }
        [Authorize]
        //account is the route
        [HttpGet]
        public decimal AccountBalance()
        {
            string userId = User.FindFirst("sub")?.Value;
            decimal userBalance = accountDao.GetBalance(userId);
            return userBalance;
            
        }
        [Authorize]
        [HttpGet("userslist")]
        public List<User> ListUsers()
        {
            string username = User.Identity.Name;
            return userDao.GetUsersNameAndId(username);
        }

        [Authorize]
        [HttpPut]
        public bool TransferTEBucks(Transfer transfer)
        {
            string userId = User.FindFirst("sub")?.Value;

            bool result = accountDao.TransferTEBucks(userId, transfer.UserIdToReceive, transfer.AmountToTransfer);
            return result;
        }
    }
}
