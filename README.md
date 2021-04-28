# Backend system
<br />
<p><b>ASP.NET Core API Backend - Bachelorprosjekt for gruppe 29</b></p>
<br />
<p>Posts: <a href="https://webforum.azurewebsites.net/posts" target="_blank">https://webforum.azurewebsites.net/posts</a></p>
<p>Comments: <a href="https://webforum.azurewebsites.net/comments" target="_blank">https://webforum.azurewebsites.net/comments</a></p>
<p>Users: <a href="https://webforum.azurewebsites.net/users" target="_blank">https://webforum.azurewebsites.net/users</a></p>
<p>Topics: <a href="https://webforum.azurewebsites.net/topics" target="_blank">https://webforum.azurewebsites.net/topics</a></p>
<p>SubTopics: <a href="https://webforum.azurewebsites.net/subtopics" target="_blank">https://webforum.azurewebsites.net/subtopics</a></p>
<p>Videos: <a href="https://webforum.azurewebsites.net/videos" target="_blank">https://webforum.azurewebsites.net/videos</a></p>
<p>InfoTopics: <a href="https://webforum.azurewebsites.net/infotopics" target="_blank">https://webforum.azurewebsites.net/infotopics</a></p>
<br />
<p><b>CustomController</b></p>
<p>Diverse metoder som brukes utenom vanlig API</p>
<br />
<p><b>GetDocuments</b> - Lister alle dokumenter som har InfoTopicId</p>
<p>Returnerer en liste med metoden: https://webforum.azurewebsites.net/GetDocuments</p>
<br />
<p><b>GetDocumentInfo</b> - Henter informasjon om et dokument (som en del av en post/kommentar)</p>
<p>Legg til dokumentId i url. F.eks: https://webforum.azurewebsites.net/GetDocumentInfo/1</p>
<br />
<p><b>GetDocument</b> - Laster ned et dokument (som en del av en post/kommentar)</p>
<p>Legg til dokumentId i url. F.eks: https://webforum.azurewebsites.net/GetDocument/1</p>
<br />
<p><b>DeleteDocument</b> - Sletter et dokument (som en del av å oppdatere post/kommentar)</p>
<p>Legg til dokumentId i url. F.eks: https://webforum.azurewebsites.net/DeleteDocument/1</p>
<br />
<p><b>UploadDocument</b> - For å laste opp nytt dokument (som en del av å oppdatere post/kommentar)</p>
<p>Her må det legges ved fil, userId og postId, commentId eller infoTopicId i et form til: https://webforum.azurewebsites.net/UploadDocument</p>
<br />
<p><b>Login</b> - For å logge inn med brukernavn eller epost</p>
<p>Legg ved username eller email og password i et form til: https://webforum.azurewebsites.net/Login</p>
<p>Denne metoden returnerer user du logger inn med som et objekt hvis ok.</p>
<br />
<p><b>SetAdmin</b> - For å legge til admin på bruker uten passord</p>
<p>Legg ved id og admin (bool) i et form til: https://webforum.azurewebsites.net/SetAdmin</p>
<br />
<p>Har også lagt til likes som action metoder</p>
<p>POST: GetLike for status: https://webforum.azurewebsites.net/GetLike</p>
<p>POST: AddLike for å legge til ny: https://webforum.azurewebsites.net/AddLike</p>
<p>DELETE: DeleteLike for å slette: https://webforum.azurewebsites.net/DeleteLike</p>
<br />
<p>Tredje utkast av Tommy</p>
