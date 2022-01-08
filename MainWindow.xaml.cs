using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BazaDateModel;
using System.Data.Entity;
using System.Data;

namespace ProiectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        BazaDateEntitiesModel ctx = new BazaDateEntitiesModel();
        CollectionViewSource customerVSource;
        CollectionViewSource movieVSource;
        CollectionViewSource reservationsVSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            customerVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerVSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();

            movieVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("movieViewSource")));
            movieVSource.Source = ctx.Movies.Local;
            ctx.Movies.Load();

            
            reservationsVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("reservationsViewSource")));
            ctx.Reservations.Load();


            cmbCustomers.ItemsSource = ctx.Customers.Local;
            cmbCustomers.SelectedValuePath = "CustId";

            cmbMovies.ItemsSource = ctx.Movies.Local;
            cmbMovies.SelectedValuePath = "MovieId";
            BindDataGrid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            SetValidationBinding();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            SetValidationBinding();

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

        }

        private void btnNextMovies_Click(object sender, RoutedEventArgs e)
        {
            movieVSource.View.MoveCurrentToNext();

        }

        private void btnPrevMovies_Click(object sender, RoutedEventArgs e)
        {
            movieVSource.View.MoveCurrentToPrevious();

        }

        private void btnPrevCustomers_Click(object sender, RoutedEventArgs e)
        {
            customerVSource.View.MoveCurrentToPrevious();

        }

        private void btnNextCustomers_Click(object sender, RoutedEventArgs e)
        {
            customerVSource.View.MoveCurrentToNext();
        }

        private void SaveCustomers()
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    
                    ctx.Customers.Add(customer);
                    customerVSource.View.Refresh();
                    
                    ctx.SaveChanges();
                }
                
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerVSource.View.Refresh();
            }

        }
        private void SaveMovies()
        {
            Movie movie = null;
            if (action == ActionState.New)
            {
                try
                {

                    movie = new Movie()
                    {
                        Title = titleTextBox.Text.Trim(),
                        Duration = durationTextBox.Text.Trim(),
                        Genre = genreTextBox.Text.Trim(),
                        Language = languageTextBox.Text.Trim(),
                        Rating = ratingTextBox.Text.Trim(),
                        ReleaseDate = releaseDateDatePicker.SelectedDate

                    };
                    
                    ctx.Movies.Add(movie);
                    movieVSource.View.Refresh();
                    
                    ctx.SaveChanges();
                }
                
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    movie = (Movie)movieDataGrid.SelectedItem;
                    movie.Title = titleTextBox.Text.Trim();
                    movie.Duration = durationTextBox.Text.Trim();
                    movie.Genre = genreTextBox.Text.Trim();
                    movie.Language = languageTextBox.Text.Trim();
                    movie.Rating = ratingTextBox.Text.Trim();
                    movie.ReleaseDate = releaseDateDatePicker.SelectedDate;
                    
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    movie = (Movie)movieDataGrid.SelectedItem;
                    ctx.Movies.Remove(movie);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerVSource.View.Refresh();
            }

        }
        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }
        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrl.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Customers":
                    SaveCustomers();
                    break;
                case "Movies":
                    SaveMovies();
                    break;
                case "Reservations":
                    SaveReservations();                    
                    break;
            }
            ReInitialize();
        }
        private void SaveReservations()
        {
            Reservation reservation = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Movie movie = (Movie)cmbMovies.SelectedItem;
                    //instantiem Order entity
                    reservation = new Reservation()
                    {

                        CustId = customer.CustId,
                        MovieId = movie.MovieId
                    };
                    
                    ctx.Reservations.Add(reservation);
                    
                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                dynamic selectedReservation = reservationsDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedReservation.ReservationId;
                    var editedreservation = ctx.Reservations.FirstOrDefault(s => s.ReservationId == curr_id);
                    if (editedreservation != null)
                    {
                        editedreservation.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        editedreservation.MovieId = Convert.ToInt32(cmbMovies.SelectedValue.ToString());
                     
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                
                reservationsVSource.View.MoveCurrentTo(selectedReservation);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedReservation = reservationsDataGrid.SelectedItem;
                    int curr_id = selectedReservation.ReservationId;
                    var deletedReservation = ctx.Reservations.FirstOrDefault(s => s.ReservationId == curr_id);
                    if (deletedReservation != null)
                    {
                        ctx.Reservations.Remove(deletedReservation);
                        ctx.SaveChanges();
                        MessageBox.Show("Reservation Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void BindDataGrid()
        {
            var queryReservation = from res in ctx.Reservations
                             join cust in ctx.Customers on res.CustId equals cust.CustId
                             join mov in ctx.Movies on res.MovieId equals mov.MovieId
                             select new
                             {
                                 res.ReservationId,
                                 res.MovieId,
                                 res.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 mov.Title,
                                 mov.Genre
                             };
            reservationsVSource.Source = queryReservation.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.imdb.com/movies-coming-soon/");
        }
        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerVSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);
            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerVSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding); 
        }
    }
}
