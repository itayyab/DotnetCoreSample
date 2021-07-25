param ($source='DotnetCoreSample\bin\Release\netcoreapp3.1\publish\*', $dest='WebAPP.zip')
Compress-Archive -Path $source -DestinationPath $dest
