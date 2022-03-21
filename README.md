# FlockChallenge

Indicaciones y aclaraciones:

- Establecer como startup el proyecto Flock.API
- El archivo users.json simula una tabla con los usuarios de la aplicación

La solución cuenta con una capa de servicios, la cual encapsula la lógica de negocio. Además tiene un proyecto Flock.Common, donde se encuentran las entidades de dominio, custom exceptions, helpers y contratos.
Para el mapeo de Models o DTO, se utiliza la librería AutoMapper.

La API está configurada con Swagger

---

Ejemplo de uso para obtener latitud y longitud:

GET: *mydomain/api/Geo/coordenadas/{provincia}*

Respuesta:
```javascript
{
	"latitud": -23.3200784211351,
	"longitud": -65.7642522180337
}
```

---

Ejemplo de uso login:

POST: *mydomain/api/Home/login*

Body Content:
```javascript
{
	"username": "a.mora",
	"password": "123456"
}
```

Respuesta:
```javascript
{
	"id": 1,
	"nombre": "Andres",
	"apellido": "Mora",
	"username": "a.mora",
	"token": "mytoken"
}
```
