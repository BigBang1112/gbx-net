using Blazored.LocalStorage;
using BlazorMonaco.Editor;
using Microsoft.JSInterop;
namespace GbxExplorerOld.Client.Sections.SectionEditor;

public class NoteManager
    {
        private const string StorageNotePrefix = "csharpEditorNote_";

        private readonly ISyncLocalStorageService localStorageService;
        private readonly IJSRuntime jsRuntime;
        private readonly StandaloneCodeEditor editor;

        public readonly Dictionary<string, Note> Notes = new();
        public Note SelectedNote { get; private set; }

        public NoteManager(ISyncLocalStorageService localStorageService, IJSRuntime jsRuntime, StandaloneCodeEditor editor)
        {
            this.localStorageService = localStorageService;
            this.jsRuntime = jsRuntime;
            this.editor = editor;

            RetrieveNotes();

            if (Notes.Count == 0)
            {
                CreateNote();
            }

            SelectedNote = Notes.Values.OrderByDescending(x => x.UpdatedAt).First();
        }
        private void RetrieveNotes()
        {
            var allKeys = localStorageService.Keys();
            foreach (var key in allKeys.Where(key => key.StartsWith(StorageNotePrefix)))
            {
                try
                {
                    var value = localStorageService.GetItem<Note>(key);
                    Notes.Add(value!.Id, value);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Invalid note: {key}");
                }
            }
        }

        public void ImportNote(Note note)
        {
            SaveNote(note);
            Notes[note.Id] = note;
        }

        public async Task DeleteNoteAsync()
        {
            var toRemove = SelectedNote;
            localStorageService.RemoveItem($"{StorageNotePrefix}{toRemove.Id}");
            Notes.Remove(SelectedNote.Id);
            if (Notes.Count > 0)
            {
                SelectedNote = Notes.Values
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault(note => note.UpdatedAt < toRemove.UpdatedAt) ?? Notes.Values
                    .First();
                await SelectNoteAsync();
            }
            else
            {
                var note = CreateNote();
                await SelectNoteAsync(note.Id);
            }
        }

        public Note CreateNote()
        {
            var counter = 1;
            while (Notes.Values.Any(note => $"new {counter}".Equals(note.Name)))
            {
                counter++;
            }
            var note = new Note(
                $"new {counter}",
                TemplateProvider.Version,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                TemplateProvider.Template);
            Notes.Add(note.Id, note);
            return note;
        }

        public async Task SelectNoteAsync()
        {
            await editor.SetValue(SelectedNote.Body);
            await jsRuntime.InvokeVoidAsync("foldAllImports");
        }

        public async Task SelectNoteAsync(string noteId)
        {
            SelectedNote = Notes[noteId];
            await SelectNoteAsync();
        }

        private void SaveNote(Note note, bool updated = true)
        {
            if (updated)
            {
                note.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
            localStorageService.SetItem($"{StorageNotePrefix}{note.Id}", note);
        }

        public async Task SaveSelectedNoteAsync(bool updated = true)
        {
            SelectedNote.Body = await editor.GetValue();
            SaveNote(SelectedNote, updated);
        }
    }
