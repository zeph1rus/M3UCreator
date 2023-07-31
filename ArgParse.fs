module M3UCreator.ArgParse

    type CommandLineOptions = {
        directory: string;
        playlist: string;
    }
    
   
    
    let defaults = {
            directory = ".";
            playlist =  "M3UCreator.m3u";
        }
       
    let rec parseOptions args workingParse =
        match args with
        | [] ->
            workingParse
        | "--directory"::nextArgs ->
            let newWorkingParse = { workingParse with directory = nextArgs.Head }
            parseOptions nextArgs newWorkingParse
        | "--playlist"::nextArgs ->
            let newWorkingParse = { workingParse with playlist = nextArgs.Head }
            parseOptions nextArgs newWorkingParse
        | arg::nextArgs ->
            parseOptions nextArgs workingParse


    let parseArgs args =
        parseOptions args defaults