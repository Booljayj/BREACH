using System.Collections.Generic;

public class Property {
	public Property() {}
	public Property(string abb, float M, float Cp) {
		this.Abb = abb;
		this.M = M;
		this.R = 8.314f/M;
		this.Cp = Cp;
		this.Cv = this.Cp-this.R;
		this.k = Cp/Cv;
	}

	public string Abb {get; private set;} // abbreviated name
	public float M {get; private set;} // kg/kmol
	public float R {get; private set;} // kJ/kg*K
	public float Cp {get; private set;} // kJ/kg*K
	public float Cv {get; private set;} // kJ/kg*K
	public float k {get; private set;} // %
}

public static class Properties {
	static public Dictionary<string, Property> properties = new Dictionary<string, Property>(){
		//These are taken at 300K, from the McGraw-Hill Gas Property Tables
		{"Null", new Property()},
		{"Argon", new Property("Ar", 39.948f, 0.5203f)},
		{"CarbonDioxide", new Property("CO2", 44.01f, 0.846f)},
		{"CarbonMonoxide", new Property("CO", 28.011f, 1.040f)},
		{"Methane", new Property("CH3", 16.043f, 2.2537f)},
		{"Nitrogen", new Property("N2", 28.013f, 1.039f)},
		{"Oxygen", new Property("O2", 31.999f, 0.918f)},
		{"Water", new Property("H2O", 18.015f, 1.8723f)},
	};

	static public Property Get(string type) {
		if (properties.ContainsKey(type))
			return properties[type];
		else
			return properties["Null"];
	}
}
