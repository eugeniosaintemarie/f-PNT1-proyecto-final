# Configuración de Google Maps API

## Instrucciones para habilitar Google Maps

Para que la validación de ubicación con Google Maps funcione correctamente, necesitas:

### 1. Obtener una API Key de Google Maps

1. Ve a [Google Cloud Console](https://console.cloud.google.com/)
2. Crea un nuevo proyecto o selecciona uno existente
3. Habilita las siguientes APIs:
   - Maps JavaScript API
   - Places API
   - Geocoding API
4. Ve a "Credenciales" y crea una nueva API Key
5. (Opcional pero recomendado) Restringe la API Key a tu dominio

### 2. Configurar la API Key en el proyecto

Abre el archivo `Views/Mascotas/Publicar.cshtml` y reemplaza `YOUR_API_KEY` con tu clave:

```html
<script src="https://maps.googleapis.com/maps/api/js?key=TU_API_KEY_AQUI&libraries=places&callback=initMap" async defer></script>
```

### 3. Funcionalidad implementada

- **Autocomplete**: Al escribir en el campo de ubicación, se sugieren direcciones de Argentina
- **Mapa interactivo**: Se muestra un mapa donde se puede ver y ajustar la ubicación
- **Marcador arrastrable**: Puedes mover el marcador para ajustar la ubicación exacta
- **Validación**: La dirección ingresada debe ser válida según Google Maps

### Nota para desarrollo local

Si estás en modo desarrollo y no quieres configurar la API ahora, puedes:
1. Comentar la línea del script de Google Maps
2. El formulario seguirá funcionando, pero sin la validación de ubicación con el mapa

---

**Archivo actualizado:** 27 de octubre de 2025
