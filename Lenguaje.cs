/*
    Melendez Chavez Ivan
*/
using System;
using System.Collections.Generic;

//Requerimiento 1.- Actualizar el dominante para variables en la expresión
//                  Ejemplo: float x; char y; y=x;  

//Requerimiento 2.- Actualizar el dominante para el Casteo y el valor de la sub expresión

//Requerimiento 3.- Programar un metodo de conversion de un valor a un tipo de dato
//                  private float convert(float valor,string tipoDato)
//                  deberan usar el residuo de la division %255, %65535

//Requerimiento 4.- Evaluar nuevamente la condicion del if-else, while, for, do while
//                  con respecto al parametro que recibe      

//Requerimiento 5.- Levantar un excepcion cuando la captura no sea un numero
//

//Requerimiento 6.- Ejecutar el For(); con el parametro que recibe

namespace semantica
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;
        public Lenguaje()
        {

        }

        public Lenguaje(string nombre) : base(nombre)
        {

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
                return valor % 255;
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


        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            Libreria();
            Variables();
            Main();
            displayVariables();
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
            BloqueInstrucciones(true);
        }

        //Bloque de instrucciones -> {ListaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "case" && getContenido() != "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion);
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion)
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion);
            }
            else if (getContenido() == "do")
            {
                Do(evaluacion);
            }
            else if (getContenido() == "for")
            {
                For(evaluacion);
            }
            else if (getContenido() == "switch")
            {
                Switch(evaluacion);
            }
            else
            {
                Asignacion(evaluacion);
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
        private void Asignacion(bool evaluacion)
        {
            log.WriteLine();
            log.Write(getContenido() + " = ");
            string NombreVar = getContenido();
            if (existeVariable(getContenido()))
            {
                match(Tipos.Identificador);
                match(Tipos.Asignacion);
                dominante = Variable.TipoDato.Char;
                Expresion();
                match(";");
                float resultado = stack.Pop();
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
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion)
        {
            match("while");
            match("(");
            bool validarWhile = Condicion();
            if (!evaluacion)
            {
                validarWhile = false;
            }
            //Requerimiento 4 
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarWhile);
            }
            else
            {
                Instruccion(evaluacion);
            }
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion)
        {
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion);
            }
            else
            {
                Instruccion(evaluacion);
            }
            match("while");
            match("(");
            //Requerimiento 4
            bool validarDo = Condicion();
            if (!evaluacion)
            {
                validarDo = false;
            }
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion)
        {
            match("for");
            match("(");
            Asignacion(evaluacion);
            bool validarFor;
            long pos = posicion;
            int lin = linea;
            do
            {
                validarFor = Condicion();
                if (!evaluacion)
                {
                    validarFor = evaluacion;
                }
                match(";");
                Incremento(validarFor);
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor);
                }
                else
                {
                    Instruccion(validarFor);
                }

                if (validarFor)
                {
                    posicion = pos-1;
                    linea = lin;
                    log.WriteLine();
                    setPosicion(posicion);
                    NextToken();
                    log.WriteLine("Repetir ciclo for");
                }
            } while (validarFor);

        }
        private void setPosicion(long posicion)
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }
        //Incremento -> Identificador ++ | --
        private void Incremento(bool evaluacion)
        {
            string variable = getContenido();
            if (existeVariable(getContenido()))
            {
                match(Tipos.Identificador);
                if (getContenido() == "++")
                {
                    match("++");
                    if (evaluacion)
                    {
                        modVariable(variable, getValor(variable) + 1);
                    }
                }
                else if (getContenido() == "--")
                {
                    match("--");
                    if (evaluacion)
                    {
                        modVariable(variable, getValor(variable) - 1);
                    }
                }
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }
        }

        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion)
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            match(")");
            match("{");
            ListaDeCasos(evaluacion);
            if (getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion);
                }
                else
                {
                    Instruccion(evaluacion);
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion)
        {
            match("case");
            Expresion();
            stack.Pop();
            match(":");
            ListaInstruccionesCase(evaluacion);
            if (getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if (getContenido() == "case")
            {
                ListaDeCasos(evaluacion);
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion()
        {
            Expresion();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            float e1 = stack.Pop();
            switch (operador)
            {
                case "==":
                    return e1 == e2;
                case ">":
                    return e1 > e2;
                case ">=":
                    return e1 >= e2;
                case "<":
                    return e1 < e2;
                case "<=":
                    return e1 <= e2;
                default:
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion)
        {
            match("if");
            match("(");
            //Requerimiento 4
            bool validarIf = Condicion();

            if(!evaluacion){
                validarIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarIf);
            }
            else
            {
                Instruccion(validarIf);
            }
            if (getContenido() == "else")
            {
                match("else");
                //Requerimiento 4
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!validarIf);
                }
                else
                {
                    Instruccion(!validarIf);
                }
            }
        }

        //Printf -> printf(cadena);
        private void Printf(bool evaluacion)
        {
            match("printf");
            match("(");
            if (getClasificacion() == Tipos.Cadena)
            {
                if (evaluacion)
                {
                    string str = getContenido();
                    string cleaned = limpiarPrints(str);
                    Console.Write(cleaned);
                }
                match(Tipos.Cadena);
            }
            else
            {
                Expresion();
                float resultado = stack.Pop();
                if (evaluacion)
                {
                    Console.Write(resultado);
                }
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, &Identificador );
        private void Scanf(bool evaluacion)
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
            }
            else
            {
                throw new Error("Error de syntaxis: variable no declarada: <" + getContenido() + "> en linea  " + linea, log);
            }


        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(operador + "  ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
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
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if (existeVariable(getContenido()))
                {
                    log.Write(getContenido());
                    log.Write(" ");

                    if (dominante < getTipo(getContenido()))
                    {
                        dominante = getTipo(getContenido());
                    }
                    stack.Push(getValor(getContenido()));
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
                Expresion();
                match(")");
                if (huboCasteo)
                {
                    float valor = stack.Pop();
                    Console.WriteLine("Casteo: " + valor + " a " + casteo);
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