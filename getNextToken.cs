        private Token getNextToken()
        {
            //u0022(comilla doble)  circunflejo u005e        
            Token token = new Token();
            bool espacio = false;
            int index;
            //Expresion regular que separa por palabras las coincidencias y mantiene en su posicion las que no
            string reservadas = @"(Main$|ImprimeTextBox$|LeeTextBox$|CreaBoton$|CreaTextbox$|CreaVentana$|def$|vent$|boton|textBox|\u005e|if$|\$|\(|\)|=|:=|;|,|\u0022.#\u0022$|Concat$|endif$|endswitch|endwhile|endfor|\{|\}|else$|" +
                @"until|repeat|Var$|int$|float|string|char|switch|for|while|" +
                @"stepnum$|initWindow|closeWindow|==|>|<|\+|-|\*|/|case|:|default|break|id$|num$)";

            //lista de posibles tokens aun no tokenizados
            List<string> tokenPrevios = new List<string>(Regex.Split(codigo, reservadas));
            tokenPrevios.RemoveAll(x => x == "" || x==" ");
      
            string posibleVar = tokenPrevios.Find(x => x.IndexOf(x) == 0);//variable candidata como valor lexico del token
            //quita espacios que puedan interferir en el analisis 
            //de variables normales excepto cadenas 
            if (posibleVar.Contains(' ') && !posibleVar.Contains("\""))
            {
                char[] c = new char[1];
                c[0] = ' ';
                string[] aux = posibleVar.Split(c, StringSplitOptions.RemoveEmptyEntries);
                posibleVar = aux[0];
                tokenPrevios.RemoveAt(0);
                tokenPrevios.Insert(0, posibleVar);
                if (aux.Length > 1)
                    tokenPrevios.Insert(1, aux[1]);                
                espacio = true;
            }
            Match match = Regex.Match(tokenPrevios.Find(x => x.IndexOf(x) == 0), reservadas);          
           
            if (match.Value !="")
            {
                if (palabraR.Exists(x => x == match.Value))//tokens de palabras reservadas tipo 2
                {
                    token = new Token(match.Value, 2);
                    index = match.Index;
                    //remueve el preToken del codigo e inserta el token ya analizado al codigo 
                    if (espacio)
                        codigo = codigo.Remove(index, match.Value.Length+1);
                    else
                        codigo = codigo.Remove(index, match.Value.Length);
                    codigo = codigo.Insert(index, token.Val);
                    return token;
                }
                else
                    return null;                
            }
            else if(tokenPrevios.Count!=0)//tokens que pueden ser id o numeros
            {

                if (posibleVar.All(c => char.IsLetter(c)) || posibleVar.Contains('"') ||
                    (posibleVar.Any(c => char.IsLetter(c)) && 
                     posibleVar.Any(c => char.IsNumber(c))))
                {
                    token = new Token(posibleVar, 1);//token id tipo 1
                }
                else if (posibleVar.All(c => char.IsNumber(c)))
                {
                    token = new Token(posibleVar, 0);//token n tipo 0
                }

                index = 0;
                //remueve el preToken del codigo e inserta el token ya analizado al codigo 
                if (espacio)
                    codigo = codigo.Remove(index, token.Val.Length + 1);
                else
                    codigo = codigo.Remove(index, posibleVar.Length);
                codigo = codigo.Insert(index, token.Nombre);

                return token;
            }
            else
            {
                return null;
            }
 } 