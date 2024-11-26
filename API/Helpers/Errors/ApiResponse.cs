namespace API.Helpers.Errors;

public class ApiResponse {
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ApiResponse(int statusCode, string message = null!) {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string GetDefaultMessageForStatusCode(int statusCode) {
        return statusCode switch {
            400 => "Has hecho una mala solicitud",
            401 => "No estás autorizado",
            404 => "El recurso no se encontró",
            405 => "Método no permitido",
            500 => "Los errores son el camino hacia el lado oscuro. Los errores llevan a la ira. La ira lleva al odio. El odio lleva a un cambio de carrera",
            _ => string.Empty
        };
    }
}
