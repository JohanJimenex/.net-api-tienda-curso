namespace API.Helpers;

public static class Autorizacion {

    public enum Roles {
        Administrador,
        Gerente,
        Empleado
    }

    public const Roles RolPredeterminado = Roles.Empleado;

}
