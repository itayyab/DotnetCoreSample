$files = Get-ChildItem ./TestResults -File -Recurse -Filter *.xml
$files= $files | select -uniq

foreach ($file in $files){
	Write-Host $files
	$path= "./TestResults/" + $file
	Write-Host $path
	Move-Item $file.FullName ($path)
}