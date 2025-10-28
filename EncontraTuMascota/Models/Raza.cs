using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

public enum Raza
{
    [Display(Name = "Labrador")]
    Labrador,
    
    [Display(Name = "Golden Retriever")]
    GoldenRetriever,
    
    [Display(Name = "Pastor Alemán")]
    PastorAleman,
    
    [Display(Name = "Bulldog")]
    Bulldog,
    
    [Display(Name = "Beagle")]
    Beagle,
    
    [Display(Name = "Poodle")]
    Poodle,
    
    [Display(Name = "Chihuahua")]
    Chihuahua,
    
    [Display(Name = "Yorkshire Terrier")]
    YorkshireTerrier,
    
    [Display(Name = "Boxer")]
    Boxer,
    
    [Display(Name = "Dálmata")]
    Dalmata,
    
    [Display(Name = "Husky Siberiano")]
    HuskySiberiano,
    
    [Display(Name = "Rottweiler")]
    Rottweiler,
    
    [Display(Name = "Cocker Spaniel")]
    CockerSpaniel,
    
    [Display(Name = "Schnauzer")]
    Schnauzer,
    
    [Display(Name = "Mestizo")]
    Mestizo,
    
    [Display(Name = "Otra")]
    Otra
}
