using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PatPilot.Models
{
    public class Gateau
    {
        public Guid Id { get; set; } 
        [Required(ErrorMessage = "Entrez Le nom")]
        [Display(Name = "Nom")]
        public string Name { get; set; } // Nom du gâteau

        [Range(0, 1000)]
        [Precision(18, 2)] // Définit la précision (18 chiffres, 2 après la virgule)
        [Required(ErrorMessage = "Entrez le prix/part du gâteau.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix/part")]
        public decimal Price { get; set; }// Prix
        public string Description { get; set; } // Description

        [Required(ErrorMessage = "Entrez L'image")]
        [Display(Name = "Image")]
        public string Image { get; set; } 
        // Relation avec les ingrédients
        public ICollection<Ingredient> Ingredients { get; set; }

        // Relation avec Enseigne
        public Guid EnseigneId { get; set; } 
        public Enseigne Enseigne { get; set; }
    }
}
