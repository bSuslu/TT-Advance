using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI_WFA
{
    // TODO- Solve Loose Coupling problem with IRequesters

    public partial class CreateTeamForm : Form
    {
        private ITeamRequester callingForm;

        private List<PersonModel> availableTeamMembers = new List<PersonModel>();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();

            callingForm = caller;

            LoadList();
            WireUpLists();
        }

        private void LoadList()
        {
            availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        }
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Ali", LastName = "Haydar" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Ekber", LastName = "Hüseyin" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Zeynel", LastName = "Abidin" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Ismail", LastName = "Hızır" });

        }
        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = firstNameTextBox.Text;
                p.LastName = lastNameTextBox.Text;
                p.EmailAddress = emailTextBox.Text;
                p.CellphoneNumber = cellphoneTextBox.Text;

                GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);
                WireUpLists();

                firstNameTextBox.Text = "";
                lastNameTextBox.Text = "";
                emailTextBox.Text = "";
                cellphoneTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill all fields");
            }
        }

        private bool ValidateForm()
        {
            if (firstNameTextBox.Text.Length == 0)
            {
                return false;
            }
            if (lastNameTextBox.Text.Length == 0)
            {
                return false;
            }
            if (emailTextBox.Text.Length == 0)
            {
                return false;
            }
            if (cellphoneTextBox.Text.Length == 0)
            {
                return false;
            }


            return true;
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();
            t.TeamName = teamNameTextBox.Text;
            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);

            this.Close();

            //TODO- if ı do not close the form, reset the form
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUpLists();
            }
        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUpLists();
            }
        }
    }
}
