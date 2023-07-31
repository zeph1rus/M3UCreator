module M3UCreator.MainModule

open System

let toLower (inStr: string) : string =
    // note for me - the () allows the compiler to figure out the return type for tolower
    // otherwise there are two methods and it gets confused
    inStr.ToLower()

let isMediaFile (filepath: string) : Boolean =
    // oh god all media formats
    match toLower (System.IO.Path.GetExtension filepath) with
    | ".mp4"
    | ".wmv"
    | ".wma"
    | ".webm"
    | ".avi"
    | ".mkv"
    | ".mov"
    | ".avchd"
    | ".mpg"
    | ".3gp"
    | ".divx"
    | ".ogm"
    | ".mp3"
    | ".aac"
    | ".alac"
    | ".ogg"
    | ".flac"
    | ".wav"
    | ".mid"
    | ".m4a"
    | ".m4b"
    | ".m4p" -> true
    | _ -> false

let writeM3u (filePath: string, filesArray: array<string>) : Boolean =
    let m3UArray = Array.append [| "#EXTM3U" |] filesArray

    try
        System.IO.File.WriteAllLines(filePath, m3UArray)
        true
    with error ->
        printfn $"Unable to write playlist: {error.Message}"
        false

let rec scanDir (path: string) : array<string> =
    let mutable newFiles = [||]

    try
        newFiles <- System.IO.Directory.GetFiles(path)

        printfn $"Scanned Directory {path} - files: {newFiles.Length}"

        let dirList = System.IO.Directory.GetDirectories(path)

        if dirList.Length > 0 then

            for d in dirList do
                let subDirFiles = scanDir d
                newFiles <- Array.append subDirFiles newFiles

        newFiles
    with error ->
        printfn $"Couldn't Read Directory {path}: {error.Message}"
        newFiles

[<EntryPoint>]
let main args =

    let config = ArgParse.parseArgs (args |> Array.toList)

    let files = (scanDir config.directory |> Array.filter isMediaFile)

    match writeM3u (config.playlist, files) with
    | true ->
        printfn $"Wrote: {files.Length} files to playlist: {config.playlist}"
        0
    | false -> -1
