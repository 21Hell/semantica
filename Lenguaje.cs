/*
    Melendez Chavez Ivan
*/
using System;
using System.Collections.Generic;

//Requerimiento 1.- Actualizacion: 
//               (X)a) Agregar el reciduo de la division en el porfactor 
//               (X)b) Agregar en intruccion los incrementos de termino y los incrementos de factor 
//                     a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1
//                     en donde el 1 puede ser una expresion 
//                (X)c) Programar el destructor
//                     para ejecutar el metodo cerrarArchivo
//                     #libreria especial? contenedor?
//                     #en la clase lexico
//Requerimiento 2.- Actualizacion la venganza:
//                  (X)c) Marcar errores semanticos cuando los incrementos de termino
//                     o icrementos de factor superen el rango de la variable
//                  (X)d) Considerar el inciso b) y c) para el for 
//                  (X)e) que funcione el do y el while
//Requerimiento 3.- Agregar:
//                (X)a) considerar las variables y los casteos de las expresiones matematicas en ensamblador
//                (X)b) considerar el residuo de la división en ensamblador, el residuo de la division queda en dx 
//                (X)c) Programar el print y scan en ensamblador
//Requerimiento 4.- Agregar:
//               (X)a) Programar else en assembler
//                  b) Programar for en assembler
//Requerimiento 5.- Agregar:
//                  PENDIETES XD
//                  a) Programar while en assembler
//                  b) Programar do while en assembler

namespace semantica
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;
        int cIf;
        int Cfor;
        int Cwhile;
        int Cdo;

        string globalIncremento = "";
        public Lenguaje()
        {
            cIf = Cfor = 0;
            Console.WriteLine("Lenguaje");
        }

        public Lenguaje(string nombre) : base(nombre)
        {
            cIf = Cfor = 0;
            Console.WriteLine("Lenguaje");
        }

        ~Lenguaje()
        {
            Console.WriteLine("Destructor");
            cerrar();
        }

        //Requerimiento 5 
        public bool esNumero(string cadena)
        {
            try
            {
                float.Parse(cadena);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //Requerimiento 3
        private float convert(float valor, string tipoDato)
        {
            if (tipoDato == "Char")
            {
                return valor % 256;
            }
            else if (tipoDato == "Int")
            {
                return valor % 65535;
            }
            else
            {
                return valor;
            }
        }
        private string limpiarPrints(string sucio)
        {
            string cleaned = sucio.TrimStart('"');
            cleaned = cleaned.Remove(cleaned.Length - 1);
            cleaned = cleaned.Replace(@"\n", "\n");
            cleaned = cleaned.Replace(@"\t", "\t");
            return cleaned;
        }
        private void addVariable(String nombre, Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }
        private void displayVariables()
        {
            log.WriteLine();
            log.WriteLine("Variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre() + " " + v.getTipo() + " " + v.getValor());
            }
        }
        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return true;
                }
            }
            return false;
        }


        private void modVariable(string nombre, float nuevoValor)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    v.setValor(nuevoValor);
                }
            }
        }

        private float getValor(string nombre)
        {
            float valorV = 0;
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    valorV = v.getValor();
                }
            }
            return valorV;
        }

        private Variable.TipoDato getTipo(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return v.getTipo();
                }
            }
            return Variable.TipoDato.Char;
        }

        private void VariablesASM(){
        {
            asm.WriteLine(";Variables: ");
            foreach (Variable v in variables)
            {
                asm.WriteLine("\t " + v.getNombre() + " DW " + v.getValor());
            }
            }
        }



        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            asm.WriteLine("#make_COM#");
            asm.WriteLine("include emu8086.inc");
            asm.WriteLine("ORG 100h");
            
            Libreria();
            Variables();
            VariablesASM();
            Main();
            displayVariables();
            asm.WriteLine("");
            
            asm.WriteLine("RET");
            asm.WriteLine("DEFINE_SCAN_NUM");
            asm.WriteLine("DEFINE_PRINT_NUM");
            asm.WriteLine("DEFINE_PRINT_NUM_UNS");
            asm.WriteLine("END");

        }

        //Librerias -> #includethrow new Error("Error lexico: No definido en linea: "+linea, log);<identificador(.h)?> Librerias?
        private void Libreria()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(Tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                //if (getContenido() == "#")
                Libreria();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variable.TipoDato tipo = Variable.TipoDato.Char;
                switch (getContenido())
                {
                    case "int": tipo = Variable.TipoDato.Int; break;
                    case "float": tipo = Variable.TipoDato.Float; break;
                }
                match(Tipos.TipoDato);
                Lista_identificadores(tipo);
                match(Tipos.FinSentencia);
                Variables();
            }
        }



        //Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato tipo)
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                {
                    addVariable(getContenido(), tipo);
                }
                else
                {
                    throw new Error("Error de syntaxis: variable duplicada: <" + getContenido() + "> en linea  " + linea, log);
                }
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }

        //Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(true, true);
        }

        //Bloque de instrucciones -> {ListaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion, bool impresion)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, impresion);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion, bool impresion)
        {
            Instruccion(evaluacion, impresion);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, impresion);
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion, bool impresion)
        {
            Instruccion(evaluacion, impresion);
            if (getContenido() != "case" && getContenido() != "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion, impresion);
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion, bool impresion)
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion, impresion);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion, impresion);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion, impresion);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion, impresion);
            }
            else if (getContenido() == "do")
            {
                Do(evaluacion, impresion);
            }
            else if (getContenido() == "for")
            {
                For(evaluacion, impresion);
            }
            else if (getContenido() == "switch")
            {
                Switch(evaluacion, impresion);
            }
            else
            {
                Asignacion(evaluacion, impresion);
            }
        }
        private Variable.TipoDato evaluaNumero(float resultado)
        {
            if (resultado % 1 != 0)
            {
                return Variable.TipoDato.Float;
            }
            if (resultado <= 255)
            {
                return Variable.TipoDato.Char;
            }
            else if (resultado <= 65535)
            {
                return Variable.TipoDato.Int;
            }
            return Variable.TipoDato.Float;
        }
        private bool evaluaSemantica(string variable, float resultado)
        {
            Variable.TipoDato tipoDato = getTipo(variable);
            return false;
        }



        //Asignacion -> identificador = cadena | Expresion;
        private void Asignacion(bool evaluacion, bool impresion)
        {
            log.WriteLine();
            log.Write(getContenido() + " = ");
            string NombreVar = getContenido();
            if (existeVariable(getContenido()))
            {
                match(Tipos.Identificador);
                dominante = Variable.TipoDato.Char;
                if (getClasificacion() == Tipos.IncrementoTermino || getClasificacion() == Tipos.IncrementoFactor)
                {
                    string incrementoTipo = getContenido();
                    if (getClasificacion() == Tipos.IncrementoTermino)
                    {
                        float resultado = Incrementotipado(NombreVar, incrementoTipo, impresion);
                        match(";");
                        if (dominante < evaluaNumero(resultado))
                        {
                            dominante = evaluaNumero(resultado);
                        }
                        if (dominante <= getTipo(NombreVar))
                        {
                            modVariable(NombreVar, resultado);
                            if (impresion)
                            {
                                asm.WriteLine(globalIncremento);
                            }
                        }
                        else
                        {
                            throw new Error("Error de semantica: variable <" + NombreVar + "> no puede ser asignada con el valor <" + resultado + "> en linea " + linea, log);
                        }
                    }
                    else
                    {
                        float resultado = Incrementotipado(NombreVar, incrementoTipo, impresion);
                        match(";");
                        if (dominante < evaluaNumero(resultado))
                        {
                            dominante = evaluaNumero(resultado);
                        }
                        if (dominante <= getTipo(NombreVar))
                        {
                            modVariable(NombreVar, resultado);
                            if (impresion)
                            {
                                asm.WriteLine(globalIncremento);
                            }
                        }
                        else
                        {
                            throw new Error("Error de semantica: variable <" + NombreVar + "> no puede ser asignada con el valor <" + resultado + "> en linea " + linea, log);
                        }
                    }
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion(impresion);
                    match(";");
                    float resultado = stack.Pop();
                    if (impresion)
                    {
                        asm.WriteLine("POP AX");
                    }
                    log.Write("= " + resultado);
                    log.WriteLine();
                    if (dominante < evaluaNumero(resultado))
                    {
                        dominante = evaluaNumero(resultado);
                    }
                    if (dominante <= getTipo(NombreVar))
                    {
                        if (evaluacion)
                        {
                            modVariable(NombreVar, resultado);
                        }
                    }
                    else
                    {
                        throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un <" + getTipo(NombreVar) + "> en linea  " + linea, log);
                    }
                    if (impresion)
                    {
                        asm.WriteLine("MOV " + NombreVar + ", AX");
                    }
                }
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }
        }

        private float Incrementotipado(string Variable, string tipoIncremento, bool impresion)
        {
            float resultado = getValor(Variable);
            globalIncremento = "";
            if (existeVariable(Variable))
            {
                switch (tipoIncremento)
                {
                    case "++":
                        match("++");
                        globalIncremento = "INC " + Variable;
                        resultado++;
                        break;
                    case "--":
                        match("--");
                        globalIncremento = "DEC " + Variable;
                        resultado--;
                        break;
                    case "+=":
                        match("+=");
                        Expresion(impresion);
                        globalIncremento = "POP AX";
                        globalIncremento += "\n" + "ADD " + Variable + ", AX";
                        resultado += stack.Pop();
                        break;
                    case "-=":
                        match("-=");
                        Expresion(impresion);
                        globalIncremento = "POP AX";
                        globalIncremento += "\n" + "SUB " + Variable + ", AX";
                        resultado -= stack.Pop();
                        break;
                    case "*=":
                        match("*=");
                        globalIncremento = "POP AX";
                        globalIncremento += "MUL " + Variable;
                        globalIncremento += "MOV " + Variable + ", AX";
                        Expresion(impresion);
                        resultado *= stack.Pop();
                        break;
                    case "/=":
                        match("/=");
                        globalIncremento = "POP AX";
                        globalIncremento += "DIV " + Variable;
                        globalIncremento += "MOV " + Variable + ", AX";

                        Expresion(impresion);
                        resultado /= stack.Pop();
                        break;
                    case "%=":
                        match("%=");
                        globalIncremento = "POP AX";
                        globalIncremento += "DIV " + Variable;
                        globalIncremento += "MOV " + Variable + ", DX";

                        Expresion(impresion);
                        resultado %= stack.Pop();
                        break;
                }
                return resultado;
            }
            return 0;

        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion, bool impresion)
        {
            if (impresion)
            {
                Cwhile++;
            }
            match("while");
            match("(");
            bool validarWhile;
            long pos = posicion;
            int lin = linea;
            string variable = getContenido();
            string etiquetaInicioWhile = "WHILEINICO" + Cwhile + ":";
            string etiquetaFinWhile = "FINWHILE" + Cwhile + ":";

            do
            {

                if (impresion)
                {
                    asm.WriteLine(etiquetaInicioWhile);
                }
                validarWhile = Condicion(etiquetaFinWhile, impresion);
                if (!evaluacion)
                {
                    validarWhile = false;
                }
                //Requerimiento 4 
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarWhile, impresion);
                }
                else
                {
                    Instruccion(evaluacion, impresion);
                }

                if (validarWhile)
                {
                    posicion = pos - variable.Length;
                    linea = lin;
                    setPosicion(posicion);
                    NextToken();
                }
                if (impresion)
                {
                    asm.WriteLine("JMP " + etiquetaInicioWhile);
                    asm.WriteLine(etiquetaFinWhile);
                }
                impresion = false;
            } while (validarWhile);

        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion, bool impresion)
        {
            if (impresion)
            {
                Cdo++;
            }

            bool validarDo;
            string etiquetaInicioDo = "DO" + Cdo + ":";
            string etiquetaFinDo = "FINDO" + Cdo + ":";


            if (!evaluacion)
            {
                validarDo = false;
            }

            match("do");
            int lin = linea;
            long pos = posicion;


            do
            {
                if (impresion)
                {
                asm.WriteLine(etiquetaInicioDo);
                }
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion, impresion);
                }
                else
                {
                    Instruccion(evaluacion, impresion);
                }
                match("while");
                match("(");
                string variable = getContenido();
                validarDo = Condicion(etiquetaFinDo, impresion);
                if (!evaluacion)
                {
                    validarDo = false;
                }
                //Requerimiento 4 
                match(")");
                match(";");
                if (validarDo)
                {
                    posicion = pos - 1;
                    linea = lin;
                    setPosicion(posicion);
                    NextToken();
                }
                if (impresion)
                {
                    asm.WriteLine("JMP " + etiquetaInicioDo);
                    asm.WriteLine(etiquetaFinDo);
                }
                impresion = false;
            } while (validarDo);
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion, bool impresion)
        {
            bool validarFor;
            if (impresion)
            {
                Cfor++;
            }
            string etiquetaInicoFor = "inicioFor" + Cfor;
            string etiquetaFinFor = "finFor" + Cfor;
            string temIncremento = globalIncremento;
            float incrementador = 0;
            match("for");
            match("(");
            Asignacion(evaluacion, impresion);


            long pos = posicion;
            int lin = linea;
            string variable = getContenido();
            float valor = getValor(variable);
            do
            {
                if (impresion)
                {
                    asm.WriteLine(etiquetaInicoFor + ":");
                }




                validarFor = Condicion(etiquetaFinFor, impresion);

                if (!evaluacion)
                {
                    validarFor = evaluacion;
                }

                match(";");
                match(Tipos.Identificador);
                incrementador = Incrementotipado(variable, getContenido(), impresion);
                //Requerimiento 1.d
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor, impresion);
                }
                else
                {
                    Instruccion(validarFor, impresion);
                }

                if (validarFor)
                {
                    modVariable(variable, incrementador);



                    posicion = pos - variable.Length;
                    linea = lin;
                    log.WriteLine();
                    setPosicion(posicion);
                    NextToken();
                    log.WriteLine("Repetir ciclo for");
                }
                if (impresion)
                {
                    asm.WriteLine(temIncremento);
                    asm.WriteLine("JMP " + etiquetaInicoFor);
                    asm.WriteLine(etiquetaFinFor + ":");
                }
                impresion = false;
            } while (validarFor);
        }
        private void setPosicion(long posicion)
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }
        //Incremento -> Identificador ++ | --
        private int Incremento(bool evaluacion, bool impresion)
        {
            string variable = getContenido();
            if (existeVariable(getContenido()))
            {
                if (getContenido() == "++")
                {
                    match("++");
                    if (evaluacion)
                    {
                        return 1;
                    }
                }
                else if (getContenido() == "--")
                {
                    match("--");
                    if (evaluacion)
                    {
                        return -1;
                    }
                }
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }
            return 0;
        }


        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion, bool impresion)
        {
            match("switch");
            match("(");
            Expresion(impresion);
            stack.Pop();
            if (impresion)
            {
                asm.WriteLine("POP AX");
            }
            match(")");
            match("{");
            ListaDeCasos(evaluacion, impresion);
            if (getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion, impresion);
                }
                else
                {
                    Instruccion(evaluacion, impresion);
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion, bool impresion)
        {
            match("case");
            Expresion(impresion);
            stack.Pop();
            asm.WriteLine("POP AX");
            match(":");
            ListaInstruccionesCase(evaluacion, impresion);
            if (getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if (getContenido() == "case")
            {
                ListaDeCasos(evaluacion, impresion);
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion(string etiqueta, bool impresion)
        {
            Expresion(impresion);
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion(impresion);
            float e2 = stack.Pop();

            float e1 = stack.Pop();
            if (impresion)
            {
                asm.WriteLine("POP BX");
                asm.WriteLine("POP AX");
                asm.WriteLine("CMP AX, BX");
            }

            switch (operador)
            {
                case "==":
                    if (impresion)
                    {
                        asm.WriteLine("JNE " + etiqueta);
                    }
                    return e1 == e2;
                case ">":
                    if (impresion)
                    {
                        asm.WriteLine("JLE " + etiqueta);
                    }
                    return e1 > e2;
                case ">=":
                    if (impresion)
                    {
                        asm.WriteLine("JL " + etiqueta);
                    }
                    return e1 >= e2;
                case "<":
                    if (impresion)
                    {
                        asm.WriteLine("JGE " + etiqueta);
                    }
                    return e1 < e2;
                case "<=":
                    if (impresion)
                    {
                        asm.WriteLine("JG " + etiqueta);
                    }
                    return e1 <= e2;
                default:
                    if (impresion)
                    {
                        asm.WriteLine("JE " + etiqueta);
                    }
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion, bool impresion)
        {
            if (impresion)
            {
                cIf++;
            }
            string etiquetaIf = "if" + cIf;
            string etiquetaElse = "else" + cIf;
            match("if");
            match("(");
            //Requerimiento 4
            bool validarIf = Condicion(etiquetaIf, impresion);
            if (!evaluacion)
            {
                validarIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarIf, impresion);
            }
            else
            {
                Instruccion(validarIf, impresion);
            }
            if (impresion)
            {
                asm.WriteLine("JMP " + etiquetaElse);
                asm.WriteLine(etiquetaIf + ":");
            }
            if (getContenido() == "else")
            {
                match("else");
                //Requerimiento 4
                if (getContenido() == "{")
                {
                    if (evaluacion)
                    {
                        BloqueInstrucciones(!validarIf, impresion);
                    }
                    else
                    {
                        BloqueInstrucciones(false, impresion);
                    }
                }
                else
                {
                    if (evaluacion)
                    {
                        Instruccion(!validarIf, impresion);
                    }
                    else
                    {
                        Instruccion(false, impresion);
                    }
                }
            }
            if (impresion)
            {
                asm.WriteLine(etiquetaElse + ":");
            }
        }

        //Printf -> printf(cadena);
        private void Printf(bool evaluacion, bool impresion)
        {
            match("printf");
            match("(");
            if (getClasificacion() == Tipos.Cadena)
            {
                string str = getContenido();
                if (evaluacion)
                {
                    
                    string cleaned = limpiarPrints(str);
                    Console.Write(cleaned);

                }
                if(impresion)
                {
                    if (str.Contains("\\n"))
                    {
                        string[] limpiada = str.Split("\\n");
                        for (int i = 0; i < limpiada.Length; i++)
                        {
                            if (i == limpiada.Length - 1)
                            {
                                asm.WriteLine("PRINT \'" + limpiada[i] + "\'");
                            }
                            else
                            {
                                asm.WriteLine("PRINTN \'" + limpiada[i] + "\'");
                            }
                        }
                    }
                    else
                    {
                        asm.WriteLine("PRINT \'" + str + "\'");
                    }
                }
                match(Tipos.Cadena);
            }
            else
            {
                Expresion(impresion);
                float resultado = stack.Pop();
                if (impresion)
                {
                    asm.WriteLine("POP AX");
                }
                if (evaluacion)
                {
                    Console.Write(resultado);

                    if (impresion)
                    {
                        asm.WriteLine("CALL PRINT_NUM");
                    }
                }
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, &Identificador );
        private void Scanf(bool evaluacion, bool impresion)
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            if (existeVariable(getContenido()))
            {
                string nombreVariable = getContenido();
                match(Tipos.Identificador);
                if (evaluacion)
                {
                    string val = "" + Console.ReadLine();
                    //Requerimiento 5
                    //validar que sea un numero
                    if (esNumero(val))
                    {
                        modVariable(nombreVariable, float.Parse(val));
                    }
                    else
                    {
                        throw new Error("Error de syntaxis: se esperaba un numero en la variable <" + nombreVariable + "> en linea  " + linea, log);
                    }
                }
                match(")");
                match(";");
                if (impresion)
                {
                    asm.WriteLine("CALL SCAN_NUM");
                    asm.WriteLine("MOV " + nombreVariable + ", CX");
                }
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }


        }

        //Expresion -> Termino MasTermino
        private void Expresion(bool impresion)
        {
            Termino(impresion);
            MasTermino(impresion);
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino(bool impresion)
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino(impresion);
                log.Write(operador + " ");
                float n1 = stack.Pop();
                if (impresion)
                {
                    asm.WriteLine("POP BX");
                }
                float n2 = stack.Pop();
                if (impresion)
                {
                    asm.WriteLine("POP AX");
                }
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        if (impresion)
                        {
                            asm.WriteLine("ADD AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        if (impresion)
                        {
                            asm.WriteLine("SUB AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "*":
                        stack.Push(n2 * n1);
                        if (impresion)
                        {
                            asm.WriteLine("MUL AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        if (impresion)
                        {
                            asm.WriteLine("DIV AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "%":
                        stack.Push(n2 % n1);
                        if (impresion)
                        {
                            asm.WriteLine("DIV AX, BX");
                            asm.WriteLine("PUSH DX");
                        }
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino(bool impresion)
        {
            Factor(impresion);
            PorFactor(impresion);
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor(bool impresion)
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor(impresion);
                log.Write(operador + "  ");
                float n1 = stack.Pop();
                if (impresion)
                {
                    asm.WriteLine("POP BX");
                }
                float n2 = stack.Pop();
                if (impresion)
                {
                    asm.WriteLine("POP AX");
                }
                //Requerimiento 1.a
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        if (impresion)
                        {
                            asm.WriteLine("MUL BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "/":
                        //Requerimiento 1.a
                        if (n1 != 0)
                        {
                            //Obtener reciduo como numero 
                            stack.Push(n2 / n1);
                            if (impresion)
                            {
                                asm.WriteLine("DIV BX");
                                asm.WriteLine("PUSH AX");
                            }
                        }
                        else
                        {
                            throw new Error("Error de syntaxis: division entre cero en linea  " + linea, log);
                        }
                        break;
                    case "%":
                        stack.Push(n2 % n1);
                        if (impresion)
                        {
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH DX");
                        }
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor(bool impresion)
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido());
                log.Write(" ");
                if (dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                if (impresion)
                {
                    asm.WriteLine("MOV AX, " + getContenido());
                    asm.WriteLine("PUSH AX");
                }
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if (existeVariable(getContenido()))
                {
                    string variable = getContenido();
                    log.Write(getContenido());
                    log.Write(" ");

                    if (dominante < getTipo(getContenido()))
                    {
                        dominante = getTipo(getContenido());
                    }
                    stack.Push(getValor(getContenido()));
                    if (impresion)
                    {
                        asm.WriteLine("MOV AX, " + variable);
                        asm.WriteLine("PUSH AX");
                    }
                    match(Tipos.Identificador);
                }
                else
                {
                    throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
                }

            }
            else
            {
                bool huboCasteo = false;
                Variable.TipoDato casteo = Variable.TipoDato.Char;
                match("(");
                if (getClasificacion() == Tipos.TipoDato)
                {
                    huboCasteo = true;
                    switch (getContenido())
                    {
                        case "char":
                            casteo = Variable.TipoDato.Char;

                            break;
                        case "int":
                            casteo = Variable.TipoDato.Int;

                            break;
                        case "float":
                            casteo = Variable.TipoDato.Float;

                            break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion(impresion);
                match(")");
                if (huboCasteo)
                {
                    dominante = casteo;
                    float valor = stack.Pop();
                    //Considerar casteo para asm
                    switch (casteo)
                    {
                        case Variable.TipoDato.Char:
                            if (impresion)
                            {
                                asm.WriteLine("POP AX");
                                asm.WriteLine("MOV AL, AH");
                                asm.WriteLine("PUSH AX");
                            }
                            break;
                        case Variable.TipoDato.Int:
                            if (impresion)
                            {
                                asm.WriteLine("POP AX");
                                asm.WriteLine("MOV AH, 0");
                                asm.WriteLine("PUSH AX");
                                asm.WriteLine("MOD AX, BX");
                                asm.WriteLine("PUSH AX");
                            }
                            break;
                        case Variable.TipoDato.Float:
                            if (impresion)
                            {
                                asm.WriteLine("POP AX");
                                asm.WriteLine("MOV AH, 0");
                                asm.WriteLine("PUSH AX");
                                asm.WriteLine("MOD AX, BX");
                                asm.WriteLine("PUSH AX");
                            }
                            break;
                    }
                    //Console.WriteLine("Casteo: " + valor + " a " + casteo);
                    stack.Push(convert(valor, casteo.ToString()));
                    //Requerimiento -> 2;
                    //Saco un elemento del stack
                    //Convierto ese valor al equivalente en casteo
                    //Requerimiento -> 3;
                    //EJ. si el casteo es (char) y el pop regresa un 256
                    // el valor equivalente en casteo es 0
                }
            }
        }
    }
}