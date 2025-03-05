using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PatPilot.Models
{

    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Identifiant unique
        public string Name { get; set; } // Nom de l'ingrédient
        public double Quantity { get; set; } // Quantité (ex. : 100g, 2 unités)
        public string Unit { get; set; } // Unité (ex. : g, ml, pièces)

        // Relation avec Gateau
        public Guid GateauId { get; set; }
        public Gateau Gateau { get; set; }
    }
}
