using System;
using System.Collections.Generic;
using System.Linq;

public class Familia : Acceso
{
    private List<Acceso> accesos = new List<Acceso>();

    /// <summary>
    /// Describe las caracter�sticas o el prop�sito del grupo.
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Familia con un componente de acceso opcional.
    /// </summary>
    /// <param name="id">Identificador �nico para la Familia.</param>
    /// <param name="nombre">Nombre de la Familia.</param>
    /// <param name="acceso">Componente de acceso opcional que se agregar� en la inicializaci�n.</param>
    public Familia(Guid id, string nombre, Acceso acceso = null)
    {
        this.Id = id;
        this.Nombre = nombre;

        if (acceso != null)
        {
            accesos.Add(acceso);
        }
    }

    /// <summary>
    /// Agrega un componente de acceso a la familia.
    /// </summary>
    /// <param name="component">El componente de acceso a agregar.</param>
    public override void Add(Acceso component)
    {
        if (component == null)
        {
            throw new ArgumentNullException(nameof(component), "El componente no puede ser nulo.");
        }
        accesos.Add(component);
    }

    /// <summary>
    /// Elimina un componente de acceso de la familia basado en su identificador �nico.
    /// </summary>
    /// <param name="component">El componente de acceso a eliminar.</param>
    public override void Remove(Acceso component)
    {
        accesos.RemoveAll(o => o.Id == component.Id);
    }

    /// <summary>
    /// Devuelve el n�mero de componentes de acceso en la familia.
    /// </summary>
    /// <returns>El n�mero de componentes de acceso.</returns>
    public override int GetCount()
    {
        return accesos.Count;
    }

    /// <summary>
    /// Proporciona acceso a la lista de componentes Acceso.
    /// </summary>
    public List<Acceso> Accesos
    {
        get { return accesos; }
    }
}
