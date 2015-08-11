using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OpenAnnounce.Data
{
    public class Club
    {
        private static readonly string[] weekdays = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        private DatabaseManager _manager;

        private int _id = -1;
        private string _name = "", _description = "", _statusMessage = "";

        private string _location = "";
        private int _teacher = -1;
        private int _weekday = 1;
        private bool _afterSchool = false;

        private DateTime _createTime = DateTime.Now, _editTime = DateTime.Now, _statusTime = DateTime.Now;

        private int _createUser = 0, _editUser = -1, _statusUser = -1;
        private int _status = 0;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }

        public string Location { get { return _location; } set { _location = value; } }
        public int TeacherId { get { return _teacher; } set { _teacher = value; } }
        public string TeacherDisplayName { get { return UserProfile.FromDatabase(_manager, _teacher).DisplayName; } }
        public int Weekday { get { return _weekday; } set { _weekday = value; } }
        public bool AfterSchool { get { return _afterSchool; } set { _afterSchool = value; } }

        public DateTime CreateTime { get { return _createTime; } set { _createTime = value; } }
        public int CreatorId { get { return _createUser; } set { _createUser = value; } }
        public string CreatorDisplayName { get { return UserProfile.FromDatabase(_manager, _createUser).DisplayName; } }

        public DateTime EditTime { get { return _editTime; } set { _editTime = value; } }
        public int EditorId { get { return _editUser; } set { _editUser = value; } }
        public string EditorDisplayName { get { return UserProfile.FromDatabase(_manager, _editUser).DisplayName; } }

        public DateTime StatusTime { get { return _statusTime; } set { _statusTime = value; } }
        public string StatusMessage { get { return _statusMessage; } set { _statusMessage = value; } }
        public int StatusUserId { get { return _statusUser; } set { _statusUser = value; } }
        public string StatusUserDisplayName { get { return UserProfile.FromDatabase(_manager, _statusUser).DisplayName; } }

        public ClubStatus Status { get { return (ClubStatus)_status; } set { _status = (int)value; } }

        public Club(DatabaseManager manager)
        {
            _manager = manager;
        }

        public Club(DatabaseManager manager, int id)
        {
            _manager = manager;
            _id = id;
        }

        public Club(DatabaseManager manager, SqlDataReader r)
        {
            _manager = manager;

            _id = (int)r["Id"];
            _name = (string)r["Name"];
            _description = (string)r["Description"];

            _location = (string)r["Location"];
            _teacher = (int)r["Teacher"];
            _weekday = (int)r["Weekday"];
            _afterSchool = (bool)r["AfterSchool"];

            _createTime = (DateTime)r["CreateTime"];
            _createUser = (int)r["CreateUser"];

            _editTime = (DateTime)r["EditTime"];
            _editUser = (r["EditUser"] is DBNull) ? -1 : (int)r["EditUser"];

            _statusTime = (DateTime)r["StatusTime"];
            _statusUser = (r["StatusUser"] is DBNull) ? -1 : (int)r["StatusUser"];
            _statusMessage = (r["StatusMessage"] is DBNull) ? "" : (string)r["StatusMessage"];

            _status = (int)r["Status"];
        }

        public void Insert()
        {
            using (SqlCommand cmd = _manager.CreateCommand("INSERT INTO Clubs (Name, Description, Location, Teacher, Weekday, AfterSchool, CreateTime, CreateUser, EditTime, EditUser, StatusTime, StatusUser, StatusMessage, Status) VALUES (@name, @description, @location, @teacher, @weekday, @afterSchool, @createTime, @createUser, @editTime, @editUser, @statusTime, @statusUser, @statusMessage, @status)"))
            {
                cmd.Parameters.AddWithValue("@name", _name);
                cmd.Parameters.AddWithValue("@description", _description);
                cmd.Parameters.AddWithValue("@location", _location);
                cmd.Parameters.AddWithValue("@teacher", _teacher);
                cmd.Parameters.AddWithValue("@weekday", _weekday);
                cmd.Parameters.AddWithValue("@afterSchool", _afterSchool);
                cmd.Parameters.AddWithValue("@createTime", _createTime);
                cmd.Parameters.AddWithValue("@createUser", _createUser);
                cmd.Parameters.AddWithValue("@editTime", _editTime);
                cmd.Parameters.AddWithValue("@editUser", (_editUser <= 0) ? DBNull.Value : (object)_editUser);
                cmd.Parameters.AddWithValue("@statusTime", _statusTime);
                cmd.Parameters.AddWithValue("@statusUser", (_statusUser <= 0) ? DBNull.Value : (object)_statusUser);
                cmd.Parameters.AddWithValue("@statusMessage", _statusMessage);
                cmd.Parameters.AddWithValue("@status", _status);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update()
        {
            using (SqlCommand cmd = _manager.CreateCommand("UPDATE Clubs SET Name=@name, Description=@description, Location=@location, Teacher=@teacher, Weekday=@weekday, AfterSchool=@afterSchool, CreateTime=@createTime, CreateUser=@createUser, EditTime=@editTime, StatusTime=@statusTime, StatusUser=@statusUser, StatusMessage=@statusMessage, Status=@status WHERE id=@id"))
            {
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.Parameters.AddWithValue("@name", _name);
                cmd.Parameters.AddWithValue("@description", _description);
                cmd.Parameters.AddWithValue("@location", _location);
                cmd.Parameters.AddWithValue("@teacher", _teacher);
                cmd.Parameters.AddWithValue("@weekday", _weekday);
                cmd.Parameters.AddWithValue("@afterSchool", _afterSchool);
                cmd.Parameters.AddWithValue("@createTime", _createTime);
                cmd.Parameters.AddWithValue("@createUser", _createUser);
                cmd.Parameters.AddWithValue("@editTime", _editTime);
                cmd.Parameters.AddWithValue("@editUser", (_editUser <= 0) ? DBNull.Value : (object)_editUser);
                cmd.Parameters.AddWithValue("@statusTime", _statusTime);
                cmd.Parameters.AddWithValue("@statusUser", (_statusUser <= 0) ? DBNull.Value : (object)_statusUser);
                cmd.Parameters.AddWithValue("@statusMessage", _statusMessage);
                cmd.Parameters.AddWithValue("@status", _status);
                cmd.ExecuteNonQuery();
            }
        }

        public string GenerateHtmlInfo()
        {
            string text = String.Empty;
            text += "<strong>Location:</strong> " + _location + "<br />";
            text += "<strong>Time:</strong> " + weekdays[_weekday] + " ";
            text += ((_afterSchool) ? "(After School)" : "(At Lunch)") + "<br />";
            text += "<strong>Responsible Teacher:</strong> " + TeacherDisplayName + "<br />";
            text += "<h1>Description</h1>" + _description;
            return text;
        }

        public static void PopulatePageNumber(DatabaseManager manager, UserProfile settings, Label currentPageLabel, Label maxPageLabel, string mode, int currentPage, int numPerPage)
        {
            using (SqlCommand cmd = manager.CreateCommand())
            {
                if (mode == "ViewAll" || mode == "Approval")
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Clubs WHERE Status<>3";
                }
                else
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Clubs WHERE Status<>3 AND CreateUser=@createUser";
                }

                cmd.Parameters.AddWithValue("@createUser", settings.Id);
                cmd.Parameters.AddWithValue("@today", DateTime.Today);

                maxPageLabel.Text = Math.Max(1, Math.Ceiling((((int)cmd.ExecuteScalar()) / (double)numPerPage))).ToString();
                currentPageLabel.Text = currentPage.ToString();
            }
        }

        public static void PopulateClubTable(DatabaseManager manager, UserProfile settings, CompiledSecurityInfo level, HtmlTable table, int offset, int rows, Dictionary<int, CheckBox> checkBoxes)
        {
            string mode;

            if (level["CanApproveClub"] && level["CanViewAllClub"])
                mode = "Approval";
            else if (level["CanViewAllClub"])
                mode = "ViewAll";
            else
                mode = "Submission";

            PopulateClubTable(manager, settings, mode, false, table, offset, rows, checkBoxes);
        }

        public static void PopulateClubTable(DatabaseManager manager, UserProfile settings, string mode, bool showDeleted, HtmlTable table, int offset, int rows, Dictionary<int, CheckBox> checkBoxes)
        {
            using (SqlCommand cmd = manager.CreateCommand())
            {
                if (mode == "ViewAll" || mode == "Approval")
                {
                    cmd.CommandText = "SELECT * FROM Clubs WHERE 1=1";
                }
                else
                {
                    cmd.CommandText = "SELECT * FROM Clubs WHERE CreateUser=@createUser";
                }

                if (!showDeleted)
                    cmd.CommandText += " AND Status<>3";

                if (mode == "Approval")
                    cmd.CommandText += " ORDER BY (CASE WHEN Status = 0 THEN 1 ELSE 0 END) DESC, ";
                else if (mode == "Submission")
                    cmd.CommandText += " ORDER BY (CASE WHEN Status = 2 THEN 1 ELSE 0 END) DESC, ";
                else
                    cmd.CommandText += " ORDER BY ";

                cmd.CommandText += "Name ASC OFFSET " + offset + " ROWS FETCH NEXT " + rows + " ROWS ONLY";

                cmd.Parameters.AddWithValue("@createUser", settings.Id);
                cmd.Parameters.AddWithValue("@weekday", DateTime.Today.DayOfWeek);

                List<Club> clubs = new List<Club>();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        while (r.Read())
                        {
                            clubs.Add(new Club(manager, r));
                        }
                    }
                    else
                    {
                        HtmlTableRow row = new HtmlTableRow();
                        HtmlTableCell cell;
                        row.Cells.Add(cell = new HtmlTableCell()
                        {
                            ColSpan = 5,
                            InnerHtml = "<em>There are currently no clubs requiring attention</em>",
                        });
                        cell.Style.Add("padding-left", "5px");
                        table.Rows.Add(row);
                    }
                }

                foreach (Club c in clubs)
                {
                    HtmlTableRow row = new HtmlTableRow();

                    if (c.Status == ClubStatus.Deleted)
                    {
                        row.Style.Add("background", "#f3f3f3");
                    }
                    else if (mode == "Approval" && c.Status == Club.ClubStatus.Pending)
                    {
                        row.Style.Add("background", "#ffa4a4");
                    }
                    else if (mode == "Submission" && c.Status == Club.ClubStatus.Denied)
                    {
                        row.Style.Add("background", "#ffa4a4");
                    }

                    HtmlTableCell checkCell;
                    CheckBox chk;

                    if (checkBoxes != null)
                    {
                        row.Cells.Add(checkCell = new HtmlTableCell());
                        checkCell.Style.Add("text-align", "center");
                        checkCell.Controls.Add(chk = new CheckBox()
                        {
                            ID = "chk_club_" + c.Id
                        });
                        if (checkBoxes.ContainsKey(c.Id))
                            checkBoxes[c.Id] = chk;
                        else
                            checkBoxes.Add(c.Id, chk);
                    }
                    row.Cells.Add(new HtmlTableCell() { InnerHtml = c.Name });
                    row.Cells.Add(new HtmlTableCell() { InnerHtml = c.CreatorDisplayName });
                    row.Cells.Add(new HtmlTableCell() { InnerHtml = weekdays[c.Weekday] });
                    switch (c.Status)
                    {
                        case Club.ClubStatus.Pending:
                            row.Cells.Add(new HtmlTableCell() { InnerHtml = "Pending" });
                            break;
                        case Club.ClubStatus.Approved:
                            row.Cells.Add(new HtmlTableCell() { InnerHtml = "Approved" });
                            break;
                        case Club.ClubStatus.Denied:
                            row.Cells.Add(new HtmlTableCell() { InnerHtml = "Denied" });
                            break;
                        case Club.ClubStatus.Deleted:
                            row.Cells.Add(new HtmlTableCell() { InnerHtml = "Deleted" });
                            break;
                        default:
                            row.Cells.Add(new HtmlTableCell() { InnerHtml = "(Unknown)" });
                            break;
                    }
                    row.Cells.Add(new HtmlTableCell() { InnerHtml = "<a href=\"ClubEdit.aspx?id=" + c.Id + "\" class=\"linkbutton-small\" style=\"padding-left: 8px; padding-right: 8px;\">Edit</a>" });

                    table.Rows.Add(row);
                }
            }
        }

        public static Club FromDatabase(DatabaseManager manager, int id)
        {
            using (SqlCommand cmd = manager.CreateCommand("SELECT * FROM Clubs WHERE Id=@id"))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return new Club(manager, r);
                    else
                        return null;
                }
            }
        }

        public enum ClubStatus : int
        {
            Pending = 0,
            Approved = 1,
            Denied = 2,
            Deleted = 3
        }
    }
}