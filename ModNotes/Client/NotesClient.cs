using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ModNotes.DbModels;
using Newtonsoft.Json;

namespace ModNotesClient
{
    public class NotesClient
    {
        readonly HttpClient _client;

        public NotesClient(HttpClient httpclient)
        {
            _client = httpclient;			
        }

        public List<Note> GetNotes()
        {
            HttpResponseMessage response = _client.GetAsync("/api/notes").Result;
            List<Note> notes = new List<Note>();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                notes = JsonConvert.DeserializeObject<System.Collections.Generic.List<Note>>(jsonString.Result);
            }
            return notes;
        }

        public Note GetNote(int id) 
        {
            HttpResponseMessage response = _client.GetAsync($"/api/notes/{id}").Result;
			if (response.IsSuccessStatusCode)
			{
                return response.Content.ReadAsAsync<Note>().Result;				
            } 
            else 
            {
                return null;
            }
        }

        public Note CreateNote(String title, String content)
        {
            Note note = new Note()
            {
                Title = title,
                Content = content
            };
            HttpResponseMessage response = _client.PostAsJsonAsync("api/notes", note).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<Note>().Result;
        }


        public Note UpdateNote(Note note)
        {
            HttpResponseMessage response = _client.PutAsJsonAsync($"api/notes/{note.Id}", note).Result;
            response.EnsureSuccessStatusCode();

            note = response.Content.ReadAsAsync<Note>().Result;
            return note;
        }

        public void DeleteNote(Note note)
        {
            HttpResponseMessage response = _client.DeleteAsync($"api/notes/{note.Id}").Result;
            response.EnsureSuccessStatusCode();
        }

    }
}