namespace API.Helpers;

public static class Roles {

    public enum RolesEnum {
        Administrador,
        Gerente,
        Empleado
    }

    public const RolesEnum RolPredeterminado = RolesEnum.Empleado;

}
