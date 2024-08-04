##########################################################################################
# Written by Tore Nestenius
# blog at https://www.nestenius.se
# Training and consulting services at https://www.tn-data.se
##########################################################################################

# This script will delete the specified resource group and all its content.

. .\Settings.ps1

write-host "Deleting the resource group '${rgname}' and all its content. This might take a while."

az group delete --name $rgname

write-host "Resource group '${rgname}' deleted."
