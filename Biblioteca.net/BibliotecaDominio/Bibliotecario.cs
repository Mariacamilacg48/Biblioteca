using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private IRepositorioLibro libroRepositorio;
        private IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {

            if (isbn != null && nombreUsuario != null)
            {
                if (!EsPrestado(isbn))
                {

                    if (!ValidarPalindromo(isbn))
                    {
                        var libro = libroRepositorio.ObtenerPorIsbn(isbn);
                        DateTime FechaInicio = DateTime.Now;
                        DateTime? FechaFinalEntrega = null;

                        // var FechaFinEntrega = ValidarFechaEntrega();

                        //Libro libro = new Libro(isbn, "maria", 19);
                        if (SumarDigitos(isbn) > 30)
                        {
                            FechaFinalEntrega = ValidarFechaEntrega();
                        }
                        Prestamo prestamo = new Prestamo(FechaInicio, libro, FechaFinalEntrega, nombreUsuario);
                        prestamoRepositorio.Agregar(prestamo);
                    }
                    else
                    {
                        Console.Write("Los libros palíndromos solo se pueden utilizar en la biblioteca");
                    }
                }
                else
                {
                    Console.Write("El libro ya se encuentra prestado");
                }
            }

        }
        public int SumarDigitos(string isbn)
        {
            int numero = 0, suma = 0;
            //SUBSTRING(isbn, 2, 3)
            string letraString = string.Empty;
            for (int i = 0; i < isbn.Length; i++)
            {
                letraString = isbn.Substring(i, 1);

                if (int.TryParse(letraString, out numero))
                {
                    suma += numero;
                }

            }
            return suma;
        }

        private DateTime? ValidarFechaEntrega()
        {
            DateTime FechaActual = DateTime.Now;
            DateTime? FechaFinalEntrega = null;

            if (FechaActual.AddDays(15).Day == DayOfWeek.Sunday.GetHashCode())
            {

                FechaFinalEntrega = FechaActual.AddDays(16);
            }

            return FechaFinalEntrega;
        }

        public bool EsPrestado(string isbn)
        {
            var libro = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

            if (libro != null)
                return true;

            return false;
        }

        public bool ValidarPalindromo(string isbn)
        {
            string isbnInvertido = string.Empty;
            int i = 0;
            int final = isbn.Length - 1;
            do
            {
                isbnInvertido = isbnInvertido + isbn.Substring(final, 1);
                i++;
                final--;
            } while (i < isbn.Length);

            if (isbn.Equals(isbnInvertido))
                return true;
            else
                return false;


        }
    }
}
