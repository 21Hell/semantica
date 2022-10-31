/*
    Melendez Chavez Ivan
*/
using System;
using System.Collections.Generic;

//Requerimiento 1.- Actualizacion: 
//               (X)a) Agregar el reciduo de la division en el porfactor 
//                  b) Agregar en intruccion los incrementos de termino y los incrementos de factor 
//                     a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1
//                     en donde el 1 puede ser una expresion 
//                  c) Programar el destructor
//                     para ejecutar el metodo cerrarArchivo
//                     #libreria especial? contenedor?
//                     #en la clase lexico
//Requerimiento 2.- Actualizacion la venganza:
//                  c) Marcar errores semanticos cuando los incrementos de termino
//                     o icrementos de factor superen el rango de la variable
//                  d) Considerar el inciso b) y c) para el for 
//                  e) que funcione el do y el while
//Requerimiento 3.- Agregar:
//                  a) considerar las variables y los casteos de las expresiones matematicas en ensamblador
//                  b) considerar el residuo de la división en ensamblador, el residuo de la division queda en dx 
//                  c) Programar el print y scan en ensamblador
//Requerimiento 4.- Agregar:
//                  a) Programar else en assembler
//                  b) Programar for en assembler
//Requerimiento 5.- Agregar:
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

        private void VariablesASM()
        {
            asm.WriteLine(";Variables");
            foreach (Variable v in variables)
            {
                asm.WriteLine("\t"+v.getNombre()+" DW ?");
            }
        }



        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            asm.WriteLine("#make_COM");
            asm.WriteLine("include 'emu8086.inc'");
            asm.WriteLine("ORG 1000h");
            Libreria();
            Variables();
            VariablesASM();
            Main();
            displayVariables();
            asm.WriteLine("RET");
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
            Console.WriteLine("Asignacion");
            log.WriteLine();
            log.Write(getContenido() + " = ");
            string NombreVar = getContenido();
            if (existeVariable(getContenido()))
            {
                match(Tipos.Identificador);
                dominante = Variable.TipoDato.Char;
                if (getClasificacion() == Tipos.IncrementoTermino || getClasificacion() == Tipos.IncrementoFactor)
                {
                    string IncrementoTipo = getContenido();
                    if(getClasificacion() == Tipos.IncrementoTermino)
                    {
                        match(Tipos.IncrementoTermino);
                        if(!(IncrementoTipo.Equals("++")||IncrementoTipo.Equals("--")))
                        {
                            Expresion();
                        }
                        switch (IncrementoTipo)
                        {
                            case "+=":
                                float resultado = stack.Pop();
                                modVariable(NombreVar, resultado);
                                asm.WriteLine("INC " + NombreVar);
                            break;
                            case "-=":
                                resultado = stack.Pop();
                                modVariable(NombreVar, -resultado);
                                asm.WriteLine("DEC " + NombreVar);
                            break;
                            case "++":
                                Console.WriteLine("Incremento");
                                modVariable(NombreVar, 1);
                                asm.WriteLine("INC " + NombreVar);
                            break;
                            case "--":
                                Console.WriteLine("Decremento");
                                modVariable(NombreVar, -1);
                                asm.WriteLine("DEC " + NombreVar);
                            break;
                        }
                        match(";");
                    }
                    else
                    {
                        match(Tipos.IncrementoFactor);
                        Expresion();
                        switch(IncrementoTipo)
                        {
                            case "*=":
                                float resultado = stack.Pop();
                                resultado = resultado * getValor(NombreVar);
                                modVariable(NombreVar, resultado);
                            break;
                            case "/=":
                                resultado = stack.Pop();
                                resultado = getValor(NombreVar) / resultado;
                                modVariable(NombreVar, resultado);
                            break;
                            case "%=":
                                resultado = stack.Pop();
                                resultado = getValor(NombreVar) % resultado;
                                modVariable(NombreVar, resultado);
                            break;
                        }
                        match(";");
                    }
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion();
                    match(";");
                    float resultado = stack.Pop();
                    asm.WriteLine("POP AX");
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
                    asm.WriteLine("MOV " + NombreVar + ", AX");
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
            bool validarWhile = Condicion("");
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
            bool validarDo = Condicion("");
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
            string etiquetaInicoFor = "inicioFor" + Cfor;
            string etiquetaFinFor = "finFor" + Cfor++;
            int incrementador = 0;
            match("for");
            match("(");
            Asignacion(evaluacion);
            bool validarFor;
            long pos = posicion;
            int lin = linea;
            string variable = getContenido();
            do
            {
                validarFor = Condicion("");
                if (!evaluacion)
                {
                    validarFor = evaluacion;
                }
                match(";");
                incrementador = Incremento(validarFor);
                //Requerimiento 1.d
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor);
                }
                else
                {
                    Instruccion(validarFor);
                }
                //If que valida si se cilca o no 
                if (validarFor)
                {
                    modVariable(variable, getValor(variable) + incrementador);
                    posicion = pos - variable.Length;
                    linea = lin;
                    log.WriteLine();
                    setPosicion(posicion);
                    NextToken();
                    log.WriteLine("Repetir ciclo for");
                }
            } while (validarFor);
            asm.WriteLine(etiquetaFinFor + ":");
        }
        private void setPosicion(long posicion)
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }
        //Incremento -> Identificador ++ | --
        private int Incremento(bool evaluacion)
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
        private void Switch(bool evaluacion)
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            asm.WriteLine("POP AX");
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
            asm.WriteLine("POP AX");    
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
        private bool Condicion(string etiqueta)
        {
            Expresion();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            asm.WriteLine("POP AX");
            float e1 = stack.Pop();
            asm.WriteLine("POP BX");
            asm.WriteLine("CMP AX, BX");
            switch (operador)
            {
                case "==":
                    asm.WriteLine("JNE " + etiqueta);
                    return e1 == e2;
                case ">":
                    asm.WriteLine("JLE " + etiqueta);
                    return e1 > e2;
                case ">=":
                    asm.WriteLine("JL " + etiqueta);
                    return e1 >= e2;
                case "<":
                    asm.WriteLine("JGE " + etiqueta);
                    return e1 < e2;
                case "<=":
                    asm.WriteLine("JG " + etiqueta);
                    return e1 <= e2;
                default:
                    asm.WriteLine("JE " + etiqueta);
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion)
        {
            string etiquetaIf = "if" + ++cIf;
            match("if");
            match("(");
            //Requerimiento 4
            bool validarIf = Condicion(etiquetaIf);
            if (!evaluacion)
            {
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
            asm.WriteLine(etiquetaIf + ":");
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
                asm.WriteLine("POP AX");
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
                asm.WriteLine("POP AX");
                float n2 = stack.Pop();
                asm.WriteLine("POP BX");
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        asm.WriteLine("ADD AX, BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        asm.WriteLine("SUB AX, BX");
                        asm.WriteLine("PUSH AX");
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
                asm.WriteLine("POP AX");
                float n2 = stack.Pop();
                asm.WriteLine("POP BX");
                //Requerimiento 1.a
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        asm.WriteLine("MUL BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "/":
                        //Requerimiento 1.a
                        if (n1 != 0)
                        {   
                            //Obtener reciduo como numero 
                            stack.Push(n2 / n1);
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH AX");
                        }
                        else
                        {
                            throw new Error("Error de syntaxis: division entre cero en linea  " + linea, log);
                        }
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
                asm.WriteLine("MOV AX,"+getContenido());
                asm.WriteLine("PUSH AX");    
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
                    dominante = casteo;
                    float valor = stack.Pop();
                    asm.WriteLine("POP AX");
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