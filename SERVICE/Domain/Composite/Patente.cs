using System;

public class Patente : Acceso
{
    /// <summary>
    /// Define el tipo de acceso otorgado por esta patente.
    /// </summary>
    public TipoAcceso TipoAcceso { get; set; }

    /// <summary>
    /// Clave de datos adicional asociada a la patente.
    /// </summary>
    public string DataKey { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Patente con un tipo de acceso específico opcional.
    /// </summary>
    /// <param name="tipoAcceso">El tipo de acceso que otorga esta patente (por defecto es TipoAcceso.UI).</param>
    public Patente(TipoAcceso tipoAcceso = TipoAcceso.UI)
    {
        this.TipoAcceso = tipoAcceso;
    }

    /// <summary>
    /// Impide la adición de subcomponentes a una hoja, lo cual no está permitido en el patrón composite.
    /// </summary>
    public override void Add(Acceso component)
    {
        throw new InvalidOperationException("No se puede agregar a una Patente, ya que es un componente hoja.");
    }

    /// <summary>
    /// Impide la eliminación de subcomponentes de una hoja, ya que no contiene ninguno.
    /// </summary>
    public override void Remove(Acceso component)
    {
        throw new InvalidOperationException("No se puede eliminar de una Patente, ya que es un componente hoja.");
    }

    /// <summary>
    /// Devuelve el número de subcomponentes, que siempre es cero para una hoja.
    /// </summary>
    public override int GetCount()
    {
        return 1; // Una Patente siempre es una única entidad.
    }
}

/// <summary>
/// Enumera los diferentes tipos de acceso que puede otorgar una patente.
/// </summary>
public enum TipoAcceso
{
    UI,
    Control,
    CasosDeUso
}
