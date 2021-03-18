# Backend system
<br />
<p><b>ASP.NET Core API Backend - Bachelorprosjekt for gruppe 29</b></p>
<br />
<p>Kommentarer: <a href="https://webforum.azurewebsites.net/comments" target="_blank">https://webforum.azurewebsites.net/comments</a></p>
<p>Poster: <a href="https://webforum.azurewebsites.net/posts" target="_blank">https://webforum.azurewebsites.net/posts</a></p>
<p>Undertema: <a href="https://webforum.azurewebsites.net/subtopics" target="_blank">https://webforum.azurewebsites.net/subtopics</a></p>
<p>Tema: <a href="https://webforum.azurewebsites.net/topics" target="_blank">https://webforum.azurewebsites.net/topics</a></p>
<p>Brukere: <a href="https://webforum.azurewebsites.net/users" target="_blank">https://webforum.azurewebsites.net/users</a></p>
<br />
<p><b>CustomController</b></p>
<p>Diverse metoder som brukes utenom vanlig API</p>
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
<p>Her må det legges ved fil, userId og postId eller commentId i et form til https://webforum.azurewebsites.net/UploadDocument</p>
<br />
<p>Tredje utkast av Tommy</p>
