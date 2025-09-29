using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P1_App1_JamesUrena.DAL;
using P1_App1_JamesUrena.Models;

namespace P1_App1_JamesUrena.Services;

public class EntradasHuacalesService(IDbContextFactory<Contexto> DbFactory)
{
    private async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .AnyAsync(e => e.EntradaId == entradaId);
    }

    private async Task<bool> Insertar(EntradasHuacales entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.EntradasHuacales.Add(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(EntradasHuacales entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(EntradasHuacales entrada)
    {
        if (!await Existe(entrada.EntradaId))
        {
            return await Insertar(entrada);
        }
        else
        {
            return await Modificar(entrada);
        }
    }

    public async Task<EntradasHuacales?> Buscar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Where(e => e.EntradaId == entradaId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<EntradasHuacales>> GetList(Expression<Func<EntradasHuacales, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
