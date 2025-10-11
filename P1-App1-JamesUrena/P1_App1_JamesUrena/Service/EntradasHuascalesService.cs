using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P1_App1_JamesUrena.DAL;
using P1_App1_JamesUrena.Models;

namespace P1_App1_JamesUrena.Services;

public class EntradasHuacalesService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .AnyAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Insertar(EntradasHuacales entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.EntradasHuacales.Add(entrada);
        await AfectarEntradaDetalle(entrada.EntradasHuacalesDetalle.ToArray(),TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarEntradaDetalle(EntradasHuacalesDetalles[] detalle, TipoOperacion tipoOperacion)
    {
        if (detalle == null || detalle.Length == 0)
            return; // evita problemas si no hay detalles

        await using var contexto = await DbFactory.CreateDbContextAsync(); 

        foreach (var item in detalle)
        {
            var tipoHuacal = await contexto.TiposHuacales
                .SingleAsync(t => t.TipoId == item.TipoId);

            if (tipoOperacion == TipoOperacion.Suma)
            {
                tipoHuacal.Existencia += item.CantidadId;
            }
            else if (tipoOperacion == TipoOperacion.Resta)
            {
                tipoHuacal.Existencia -= item.CantidadId;
            }
        }
        await contexto.SaveChangesAsync();
    }


    public async Task<bool> Modificar(EntradasHuacales entradasHuacales)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entradaAnterior = await contexto.EntradasHuacales
            .Include(e => e.EntradasHuacalesDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entradasHuacales.EntradaId);

        if (entradaAnterior == null)
        {
            return false;
        }


        await AfectarEntradaDetalle(entradaAnterior.EntradasHuacalesDetalle.ToArray(), TipoOperacion.Resta);


        await AfectarEntradaDetalle(entradasHuacales.EntradasHuacalesDetalle.ToArray(), TipoOperacion.Suma);

        contexto.EntradasHuacales.Update(entradasHuacales);
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
            .Include(e => e.EntradasHuacalesDetalle)
                .ThenInclude(d => d.TipoHuacales)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Eliminar(int idEntrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entrada = await contexto.EntradasHuacales
            .Include(e => e.EntradasHuacalesDetalle)
            .FirstOrDefaultAsync(e => e.EntradaId == idEntrada);

        if (entrada == null)
        {
            return false;
        }

        await AfectarEntradaDetalle(entrada.EntradasHuacalesDetalle.ToArray(), TipoOperacion.Resta);

        contexto.EntradasHuacalesDetalles.RemoveRange(entrada.EntradasHuacalesDetalle);
        contexto.EntradasHuacales.Remove(entrada);

        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<List<EntradasHuacales>> GetList(Expression<Func<EntradasHuacales, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Include(d => d.EntradasHuacalesDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
    public async Task<List<TiposHuacales>> ListarTiposHuacales()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.TiposHuacales
            .AsNoTracking()
            .ToListAsync();
    }
}
