using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OpenAnnounce.Data;

namespace OpenAnnounce.Admin
{
    public partial class Default : AnnouncementsPage
    {
        private List<NavbarEditRow> NavbarEditRows = new List<NavbarEditRow>();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }

            NewAnnouncement.Visible = CurrentUser.SecurityAccess["CanSubmitAnnouncement"];
            NewClub.Visible = CurrentUser.SecurityAccess["CanSubmitClub"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateProfile();

            while (AnnouncementTable.Rows.Count > 1)
                AnnouncementTable.Rows.RemoveAt(1);
            while (ClubTable.Rows.Count > 1)
                ClubTable.Rows.RemoveAt(1);

            Announcement.PopulateAnnouncementTable(DatabaseManager.Current, CurrentUser.Profile, CurrentUser.SecurityAccess, AnnouncementTable, 0, 5, null);
            Club.PopulateClubTable(DatabaseManager.Current, CurrentUser.Profile, CurrentUser.SecurityAccess, ClubTable, 0, 5, null);

            if (CurrentUser.SecurityAccess["CanEditNavbar"])
            {
                PopulateNavbarEdit();
            }
            else
            {
                NavbarEdit.Visible = false;
            }
        }

        private void PopulateNavbarEdit()
        {
            using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM NavbarLinks"))
            {
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        NavbarEditRows.Add(new NavbarEditRow(r, this));
                    }
                }
            }

            for (int i = 0; i < NavbarEditRows.Count; i++)
            {
                NavbarEditRows[i].GenerateFields();
                NavbarTable.Rows.Add(NavbarEditRows[i].GenerateRow());
            }
        }

        private void PopulateProfile()
        {
            Username.Text = CurrentUser.Profile.Username;
            DisplayName.Text = CurrentUser.Profile.DisplayName;
        }

        private void NavbarSave_Click(object sender, EventArgs e)
        {
            foreach (NavbarEditRow row in NavbarEditRows)
            {
                if (row.SaveButton == sender)
                {
                    if (!row.UrlBox.Text.Contains(':'))
                        row.UrlBox.Text = "http://" + row.UrlBox.Text;

                    try
                    {
                        row.CommitToDatabase();
                        ShowMessage("Navbar link has been saved successfully.", "message-success");
                    }
                    catch (FormatException)
                    {
                        ShowMessage("Invalid scope. Please contact an administrator for assistance.", "message-error");
                    }
                }
            }
        }

        private void NavbarDelete_Click(object sender, EventArgs e)
        {
            NavbarEditRow row;
            for (int i = 0; i < NavbarEditRows.Count; i++)
            {
                row = NavbarEditRows[i];
                if (row.DeleteButton == sender)
                {
                    using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("DELETE FROM NavbarLinks WHERE Id=@id"))
                    {
                        cmd.Parameters.AddWithValue("@id", row.Id);
                        cmd.ExecuteNonQuery();
                        NavbarEditRows.RemoveAt(i);
                        NavbarTable.Rows.RemoveAt(i + 1);
                        ShowMessage("That link has been removed from the navbar.", "message-success");
                        return;
                    }
                }
            }
        }

        private void ShowMessage(string message, string cssClass)
        {
            Message.Visible = true;
            Message.Attributes["class"] = "message-base " + cssClass;
            Message.InnerHtml = message;
        }

        protected void NavbarAddButton_Click(object sender, EventArgs e)
        {
            NavbarEditRow row = NavbarEditRow.NewLink(this);
            row.GenerateFields();
            NavbarTable.Rows.Add(row.GenerateRow());
            NavbarEditRows.Add(row);
        }

        public class NavbarEditRow
        {
            private int _id, _scope;
            private string _text, _url;
            private TextBox _textBox, _urlBox;
            private DropDownList _scopeBox;
            private LinkButton _saveButton, _deleteButton;

            public int Id { get { return _id; } }

            public TextBox TextBox { get { return _textBox; } }
            public TextBox UrlBox { get { return _urlBox; } }
            public DropDownList ScopeBox { get { return _scopeBox; } }

            public LinkButton SaveButton { get { return _saveButton; } }
            public LinkButton DeleteButton { get { return _deleteButton; } }

            public NavbarEditRow(SqlDataReader r, Default page)
            {
                _id = (int)r["Id"];
                _text = (string)r["Text"];
                _url = (string)r["URL"];
                _scope = (r["Scope"] is DBNull) ? 0 : (int)r["Scope"];
                _saveButton = new LinkButton()
                {
                    Text = "OK",
                    CssClass = "linkbutton linkbutton-small",
                    ID = "btn_save_" + _id
                };
                _saveButton.Click += page.NavbarSave_Click;
                _deleteButton = new LinkButton()
                {
                    Text = "-",
                    CssClass = "linkbutton linkbutton-small",
                    ID = "btn_del_" + _id
                
                };
                _deleteButton.Click += page.NavbarDelete_Click;
            }

            public void GenerateFields()
            {
                _textBox = new TextBox()
                {
                    Text = _text,
                    Width = new Unit("98%"),
                    ID = "txt_text_" + _id
                };
                _urlBox = new TextBox()
                {
                    Text = _url,
                    Width = new Unit("98%"),
                    ID = "txt_url_" + _id
                };
                _scopeBox = new DropDownList()
                {
                    Width = new Unit("98%"),
                    ID = "sel_scope_" + _id
                };

                foreach (Scope s in Scope.AllFromDatabase(DatabaseManager.Current))
                {
                    _scopeBox.Items.Add(new ListItem(s.Name, s.Id.ToString()));
                    if (_scope == s.Id)
                        _scopeBox.SelectedIndex = _scopeBox.Items.Count - 1;
                }
            }

            public HtmlTableRow GenerateRow()
            {
                HtmlTableRow row = new HtmlTableRow();
                HtmlTableCell cell;

                row.Cells.Add(cell = new HtmlTableCell());
                cell.Controls.Add(_textBox);

                row.Cells.Add(cell = new HtmlTableCell());
                cell.Controls.Add(_urlBox);

                row.Cells.Add(cell = new HtmlTableCell());
                cell.Controls.Add(_scopeBox);

                row.Cells.Add(cell = new HtmlTableCell());
                cell.Controls.Add(_saveButton);
                cell.Controls.Add(new Literal()
                {
                    Text = " "
                });
                cell.Controls.Add(_deleteButton);

                return row;
            }

            public void CommitToDatabase()
            {
                using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("UPDATE NavbarLinks SET Text=@text, URL=@url, Scope=@scope WHERE Id=@id"))
                {
                    cmd.Parameters.AddWithValue("@text", _textBox.Text);
                    cmd.Parameters.AddWithValue("@url", _urlBox.Text);
                    cmd.Parameters.AddWithValue("@scope", (_scopeBox.SelectedValue == "0") ? DBNull.Value : (object)Int32.Parse(_scopeBox.SelectedValue));
                    cmd.Parameters.AddWithValue("@id", _id);
                    cmd.ExecuteNonQuery();
                }
            }

            public static NavbarEditRow NewLink(Default page)
            {
                int id;

                using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("INSERT INTO NavbarLinks (Text, URL, Scope) OUTPUT INSERTED.Id VALUES ('Example', 'http://www.example.com/', NULL)"))
                {
                    id = (int)cmd.ExecuteScalar();
                }


                using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM NavbarLinks WHERE Id=@id"))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                            return new NavbarEditRow(r, page);
                        else
                            return null;
                    }
                }
            }
        }
    }
}