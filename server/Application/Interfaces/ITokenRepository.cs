using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Application.DAO;

namespace Application.Interfaces;
public interface ITokenRepository
{
    Task AddTokenAsync(TokenDAO token);
    Task<TokenDAO> GetTokenByValueAsync(string tokenValue);
    Task<List<TokenDAO>> GetTokensByUserIdAsync(string userId);
}