internal class Material {

    internal readonly float KAmbient;
    internal readonly float KDiffuse;
    internal readonly float KSpecular;
    internal readonly float Shininess;

    internal Material(float KAmbient, float KDiffuse, float KSpecular, float Shininess) {
        this.KAmbient = KAmbient;
        this.KDiffuse = KDiffuse;
        this.KSpecular = KSpecular;
        this.Shininess = Shininess;
    }

    public static Material Default() => new Material(0.3f,0.7f,0.7f,10f);

}