#!/bin/bash

RESULT=1

while [ $RESULT -gt 0 ]
do
    echo "MIGRATION ATTEMPT"
    dotnet ef database update
    RESULT=$?
    echo $RESULT
    sleep 5
done

echo "MIGRATION DONE"