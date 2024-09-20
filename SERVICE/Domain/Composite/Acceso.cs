using System;

public abstract class Acceso
{
    /// <summary>
    /// Identificador único para el componente de acceso.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del componente de acceso.
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Acceso.
    /// </summary>
    public Acceso()
    {
    }

    /// <summary>
    /// Agrega un componente hijo al componente actual.
    /// </summary>
    /// <param name="component">El componente hijo que se va a agregar.</param>
    public abstract void Add(Acceso component);

    /// <summary>
    /// Elimina un componente hijo del componente actual.
    /// </summary>
    /// <param name="component">El componente hijo que se va a eliminar.</param>
    public abstract void Remove(Acceso component);

    /// <summary>
    /// Devuelve el número de componentes hijos.
    /// </summary>
    /// <returns>El número de componentes hijos.</returns>
    public abstract int GetCount();
}
