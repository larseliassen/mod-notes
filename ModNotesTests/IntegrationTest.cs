using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModNotes.DbModels;
using ModNotesClient;

namespace ModNotesTests
{
    [TestClass]
    public class IntegrationTest
    {
		TestServer _server;
        NotesClient _client;
        HttpClient _httpClient;

        [TestInitialize]
        public void SetUp() 
        {
			_server = new TestServer(new WebHostBuilder().UseStartup<ModNotes.Startup>());
			_httpClient = _server.CreateClient();
			_client = new NotesClient(_httpClient);   
        }
			

        [TestMethod]
        public void ShouldCreateNewNotes()
        {            
            _client.CreateNote("Note 1", "This is content of note number 1.");
            Assert.AreEqual(1, _client.GetNotes().Count);
        }

        [TestMethod]
		public void ShouldGetSingleNotes()
		{
			Note note1 = _client.CreateNote("Note 1", "This is content of note number 1.");
            Note note2 = _client.CreateNote("Note 2", "This is content of note number 2.");
			
            Assert.IsTrue(note1.Equals(_client.GetNote(note1.Id)));
		}

        [TestMethod]
        public void GetNonExistingNoteReturnsNull()
        {
            Assert.IsNull(_client.GetNote(1));
        }

		[TestMethod]
		public void ShouldListNotes()
		{
			_client.CreateNote("Note 1", "This is content of note number 1.");
            _client.CreateNote("Note 2", "This is content of note number 2.");
            _client.CreateNote("Note 3", "This is content of note number 3.");
			Assert.AreEqual(3, _client.GetNotes().Count);
		}

		[TestMethod]
		public void ShouldUpdateNote()
		{
			Note note = _client.CreateNote("New note", "This is the content before update.");			
            Assert.IsTrue(_client.GetNotes().Find(n => n.Id.Equals(note.Id)).Content.Equals("This is the content before update."));

            note.Content = "This is the content after update.";
            _client.UpdateNote(note);
            Assert.IsTrue(_client.GetNotes().Find(n => n.Id.Equals(note.Id)).Content.Equals("This is the content after update."));
		}

		[TestMethod]
		public void UpdateNonExistingNoteThrowsException()
		{
            Note note = new Note()
            {
                Title = "Nonexisting",
                Content = "Nonexisting note"
            };
            Assert.ThrowsException<HttpRequestException>(() => _client.DeleteNote(note));
		}

		[TestMethod]
		public void ShouldDeleteNote()
		{
			Note note = _client.CreateNote("New note", "This is the note to be deleted.");
			Assert.IsTrue(_client.GetNotes().Find(n => n.Id.Equals(note.Id)).Content.Equals("This is the note to be deleted."));

			
            _client.DeleteNote(note);
            Assert.IsNull(_client.GetNotes().Find(n => n.Id.Equals(note.Id)));
		}

		[TestMethod]
		public void DeleteNonExistingThrowsException()
		{
			Note note = new Note()
			{
				Title = "Nonexisting",
				Content = "Nonexisting note"
			};
            Assert.ThrowsException<HttpRequestException>(() => _client.DeleteNote(note));
		}

		[TestCleanup]
		public void CleanUp()
		{
			_server.Dispose();
			_httpClient.Dispose();
		}
    }
}
